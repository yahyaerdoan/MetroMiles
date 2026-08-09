using MediatR;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using System.Text.Json.Serialization;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.RevokeSession;

public class RevokeSessionCommand : IRequest<OperationResult>
{
    public Guid? SessionId { get; set; }

    // Set by the controller from the caller's own JWT claims.
    [JsonIgnore]
    public Guid UserId { get; set; }

    // Set by the controller from the request's actual remote IP.
    [JsonIgnore]
    public string IpAddress { get; set; } = string.Empty;

    public class RevokeSessionCommandHandler(IRefreshTokenRepository refreshTokenRepository) : IRequestHandler<RevokeSessionCommand, OperationResult>
    {
        // Same message for "doesn't exist" and "belongs to someone else" — avoids confirming ids.
        private const string SessionNotFoundMessage = "Session not found.";

        public async Task<OperationResult> Handle(RevokeSessionCommand request, CancellationToken cancellationToken)
        {
            var session = await refreshTokenRepository.GetAsync(
                predicate: rt => rt.Id == request.SessionId && rt.UserId == request.UserId,
                cancellationToken: cancellationToken);

            if (session is null)
            {
                return Result.NotFound(SessionNotFoundMessage);
            }

            if (session.Revoked is not null)
            {
                return Result.BadRequest("Session is already revoked.");
            }

            session.Revoked = DateTime.UtcNow;
            session.RevokedByIp = request.IpAddress;
            await refreshTokenRepository.UpdateAsync(session);

            return Result.Success();
        }
    }
}
