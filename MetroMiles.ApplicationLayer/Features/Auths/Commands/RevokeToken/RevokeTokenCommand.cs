using MediatR;

using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.RevokeToken;

public class RevokeTokenCommand : IRequest<OperationResult>
{
    public string Token { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;

    public class RevokeTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository) : IRequestHandler<RevokeTokenCommand, OperationResult>
    {
        public async Task<OperationResult> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var existingToken = await refreshTokenRepository.GetAsync(
                predicate: rt => rt.Token == request.Token,
                cancellationToken: cancellationToken);

            if (existingToken is null || existingToken.Revoked is not null || existingToken.Expires <= DateTime.UtcNow)
            {
                return Result.Unauthorized("Invalid or expired refresh token.");
            }

            existingToken.Revoked = DateTime.UtcNow;
            existingToken.RevokedByIp = request.IpAddress;
            await refreshTokenRepository.UpdateAsync(existingToken);

            return Result.Success();
        }
    }
}
