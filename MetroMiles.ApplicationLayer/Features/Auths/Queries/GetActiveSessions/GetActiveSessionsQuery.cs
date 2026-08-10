using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Auths.Queries.GetSessionHistory;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using System.Text.Json.Serialization;

namespace MetroMiles.ApplicationLayer.Features.Auths.Queries.GetActiveSessions;

public class GetActiveSessionsQuery : IRequest<OperationDataResult<GetListResponse<SessionHistoryItemResponse>>>
{
    // Set by the controller from the caller's own JWT claims.
    [JsonIgnore]
    public Guid UserId { get; set; }

    public required PageRequest PageRequest { get; set; }

    public class GetActiveSessionsQueryHandler(IRefreshTokenRepository refreshTokenRepository)
        : IRequestHandler<GetActiveSessionsQuery, OperationDataResult<GetListResponse<SessionHistoryItemResponse>>>
    {
        public async Task<OperationDataResult<GetListResponse<SessionHistoryItemResponse>>> Handle(GetActiveSessionsQuery request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var sessions = await refreshTokenRepository.GetListAsync(
                predicate: rt => rt.UserId == request.UserId && rt.Revoked == null && rt.Expires > now,
                orderBy: q => q.OrderByDescending(rt => rt.CreatedDate),
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken);

            GetListResponse<SessionHistoryItemResponse> response = new()
            {
                Items = [.. sessions.Items.Select(ToResponse)],
                Index = sessions.Index,
                Size = sessions.Size,
                Count = sessions.Count,
                Pages = sessions.Pages,
                HasPrevious = sessions.HasPrevious,
                HasNext = sessions.HasNext,
            };

            return Result.Success(response);
        }

        private static SessionHistoryItemResponse ToResponse(RefreshToken refreshToken) => new()
        {
            Id = refreshToken.Id,
            CreatedDate = refreshToken.CreatedDate,
            CreatedByIp = refreshToken.CreatedByIp,
            Expires = refreshToken.Expires,
            Revoked = refreshToken.Revoked,
            RevokedByIp = refreshToken.RevokedByIp,
            IsActive = true,
        };
    }
}
