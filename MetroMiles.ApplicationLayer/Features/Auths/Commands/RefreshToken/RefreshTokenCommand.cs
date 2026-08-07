using Core.SecurityLayer.Entities;
using Core.SecurityLayer.Hashings;
using Core.SecurityLayer.JsonWebTokens.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Auths.Commands.Login;
using MetroMiles.ApplicationLayer.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<OperationDataResult<LoggedResponse>>
{
    public string Token { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;

    public class RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, IJwtTokenHelper jwtTokenHelper)
        : IRequestHandler<RefreshTokenCommand, OperationDataResult<LoggedResponse>>
    {
        private const string InvalidRefreshTokenMessage = "Invalid or expired refresh token.";

        public async Task<OperationDataResult<LoggedResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var hashedToken = TokenHashingHelper.Hash(request.Token);
            var existingToken = await refreshTokenRepository.GetAsync(
                predicate: rt => rt.Token == hashedToken,
                include: q => q.Include(rt => rt.User).ThenInclude(u => u.UserOperationClaims).ThenInclude(uoc => uoc.OperationClaim),
                cancellationToken: cancellationToken);

            if (existingToken is null || existingToken.Revoked is not null || existingToken.Expires <= DateTime.UtcNow)
            {
                return Result.Unauthorized<LoggedResponse>(InvalidRefreshTokenMessage);
            }

            var user = existingToken.User;
            var (newRefreshToken, rawNewRefreshToken) = jwtTokenHelper.CreateRefreshToken(user, request.IpAddress);

            existingToken.Revoked = DateTime.UtcNow;
            existingToken.RevokedByIp = request.IpAddress;
            existingToken.ReplacedByToken = newRefreshToken.Token;
            await refreshTokenRepository.UpdateAsync(existingToken);
            await refreshTokenRepository.AddAsync(newRefreshToken);

            IList<OperationClaim> operationClaims = [.. user.UserOperationClaims.Select(uoc => uoc.OperationClaim)];
            var accessToken = jwtTokenHelper.CreateToken(user, operationClaims);

            return Result.Success(new LoggedResponse(accessToken.Token, accessToken.Expiration, rawNewRefreshToken));
        }
    }
}
