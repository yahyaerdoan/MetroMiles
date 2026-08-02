using Core.SecurityLayer.Entities;
using Core.SecurityLayer.Hashings;
using Core.SecurityLayer.JsonWebTokens.Abstractions;
using Core.SecurityLayer.JsonWebTokens.Concretions;

using MediatR;

using MetroMiles.ApplicationLayer.Services.Repositories;

using Microsoft.EntityFrameworkCore;

using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.Login;

public class LoginCommand : IRequest<OperationDataResult<LoggedResponse>>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;

    public class LoginCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IJwtTokenHelper jwtTokenHelper, TokenOption tokenOptions)
        : IRequestHandler<LoginCommand, OperationDataResult<LoggedResponse>>
    {
        // Deliberately generic: distinguishing "no such user" from "wrong password" here lets an
        // attacker enumerate registered emails via the login endpoint's response.
        private const string InvalidCredentialsMessage = "Invalid email or password.";

        public async Task<OperationDataResult<LoggedResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var normalizedEmail = request.Email.ToUpperInvariant();
            var user = await userRepository.GetAsync(
                predicate: u => u.NormalizedEmail == normalizedEmail,
                include: q => q.Include(u => u.UserOperationClaims).ThenInclude(uoc => uoc.OperationClaim),
                cancellationToken: cancellationToken);

            if (user is null || !HashingHelper.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Result.Unauthorized<LoggedResponse>(InvalidCredentialsMessage);
            }

            // Checked only after a successful password match, so a disabled account's status isn't
            // revealed to someone who doesn't already know the password.
            if (!user.Status)
            {
                return Result.Forbidden<LoggedResponse>("This account is disabled.");
            }

            // Lazy migration off the legacy HMACSHA512 scheme: we already have the plaintext password
            // right here (the only place we ever do), so silently re-hash it with PBKDF2 now instead
            // of forcing every existing user through a password reset.
            if (HashingHelper.IsLegacyHash(user.PasswordSalt))
            {
                HashingHelper.CreatePasswordHash(request.Password, out var rehashedPassword, out var rehashedSalt);
                user.PasswordHash = rehashedPassword;
                user.PasswordSalt = rehashedSalt;
                await userRepository.UpdateAsync(user);
            }

            // Housekeeping: revoke any refresh tokens this user still holds that have outlived the
            // configured refresh TTL window but were never used to refresh or explicitly revoked.
            var staleTokens = await refreshTokenRepository.GetOldRefreshTokensAsync(user.Id, tokenOptions.RefreshTokenTTL);
            foreach (var staleToken in staleTokens)
            {
                staleToken.Revoked = DateTime.UtcNow;
                staleToken.RevokedByIp = request.IpAddress;
                await refreshTokenRepository.UpdateAsync(staleToken);
            }

            IList<OperationClaim> operationClaims = [.. user.UserOperationClaims.Select(uoc => uoc.OperationClaim)];
            var accessToken = jwtTokenHelper.CreateToken(user, operationClaims);

            var refreshToken = jwtTokenHelper.CreateRefreshToken(user, request.IpAddress);
            await refreshTokenRepository.AddAsync(refreshToken);

            return Result.Success(new LoggedResponse(accessToken.Token, accessToken.Expiration, refreshToken.Token));
        }
    }
}
