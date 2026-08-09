using Core.SecurityLayer.JsonWebTokens.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Auths.Services;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using System.Text.Json.Serialization;
using RefreshTokenEntity = MetroMiles.DomainLayer.Entities.RefreshToken;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.Login;

public class LoginCommand : IRequest<OperationDataResult<LoggedResponse>>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    // Set by the controller from the request's actual remote IP.
    [JsonIgnore]
    public string IpAddress { get; set; } = string.Empty;

    public class LoginCommandHandler(UserManager<User> userManager, IRefreshTokenRepository refreshTokenRepository, IJwtTokenHelper jwtTokenHelper, UserClaimsFactory userClaimsFactory)
        : IRequestHandler<LoginCommand, OperationDataResult<LoggedResponse>>
    {
        private const string InvalidCredentialsMessage = "Invalid email or password.";

        // Deliberately generic: distinguishing "no such user" from "wrong password" here lets an
        // attacker enumerate registered emails via the login endpoint's response.
        public async Task<OperationDataResult<LoggedResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                return Result.Unauthorized<LoggedResponse>(InvalidCredentialsMessage);
            }

            if (await userManager.IsLockedOutAsync(user))
            {
                return Result.Forbidden<LoggedResponse>("This account is locked out.");
            }

            var staleTokens = await refreshTokenRepository
                .GetListAsync(predicate: rt => rt.UserId == user.Id && rt.Revoked == null && rt.Expires <= DateTime.UtcNow, size: 100, cancellationToken: cancellationToken);

            foreach (var staleToken in staleTokens.Items)
            {
                staleToken.Revoked = DateTime.UtcNow;
                staleToken.RevokedByIp = request.IpAddress;
                await refreshTokenRepository.UpdateAsync(staleToken);
            }

            var claims = await userClaimsFactory.CreateClaimsAsync(user);
            var accessToken = jwtTokenHelper.CreateToken(claims);

            var refreshTokenResult = jwtTokenHelper.CreateRefreshToken();
            await refreshTokenRepository.AddAsync(new RefreshTokenEntity(user.Id, refreshTokenResult.HashedToken, refreshTokenResult.Expires, request.IpAddress));

            return Result.Success(new LoggedResponse(accessToken.Token, accessToken.Expiration, refreshTokenResult.RawToken));
        }
    }
}
