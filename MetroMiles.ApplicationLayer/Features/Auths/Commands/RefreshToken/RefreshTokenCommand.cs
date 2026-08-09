using Core.SecurityLayer.Hashings;
using Core.SecurityLayer.JsonWebTokens.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Auths.Commands.Login;
using MetroMiles.ApplicationLayer.Features.Auths.Services;
using MetroMiles.ApplicationLayer.Services.Repositories;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using RefreshTokenEntity = MetroMiles.DomainLayer.Entities.RefreshToken;
using User = MetroMiles.DomainLayer.Entities.User;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<OperationDataResult<LoggedResponse>>
{
    public string Token { get; set; } = string.Empty;

    public string IpAddress { get; set; } = string.Empty;

    public class RefreshTokenCommandHandler(
        UserManager<User> userManager,
        IRefreshTokenRepository refreshTokenRepository,
        IJwtTokenHelper jwtTokenHelper,
        UserClaimsFactory userClaimsFactory)
        : IRequestHandler<RefreshTokenCommand, OperationDataResult<LoggedResponse>>
    {
        private const string InvalidRefreshTokenMessage = "Invalid or expired refresh token.";

        public async Task<OperationDataResult<LoggedResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var hashedToken = TokenHashingHelper.Hash(request.Token);
            var existingToken = await refreshTokenRepository.GetAsync(
                predicate: rt => rt.Token == hashedToken,
                cancellationToken: cancellationToken);

            if (existingToken is null || existingToken.Revoked is not null || existingToken.Expires <= DateTime.UtcNow)
            {
                return Result.Unauthorized<LoggedResponse>(InvalidRefreshTokenMessage);
            }

            var user = await userManager.FindByIdAsync(existingToken.UserId.ToString());
            if (user is null)
            {
                return Result.Unauthorized<LoggedResponse>(InvalidRefreshTokenMessage);
            }

            var refreshTokenResult = jwtTokenHelper.CreateRefreshToken();

            existingToken.Revoked = DateTime.UtcNow;
            existingToken.RevokedByIp = request.IpAddress;
            existingToken.ReplacedByToken = refreshTokenResult.HashedToken;
            await refreshTokenRepository.UpdateAsync(existingToken);
            await refreshTokenRepository.AddAsync(new RefreshTokenEntity(user.Id, refreshTokenResult.HashedToken, refreshTokenResult.Expires, request.IpAddress));

            var claims = await userClaimsFactory.CreateClaimsAsync(user);
            var accessToken = jwtTokenHelper.CreateToken(claims);

            return Result.Success(new LoggedResponse(accessToken.Token, accessToken.Expiration, refreshTokenResult.RawToken));
        }
    }
}
