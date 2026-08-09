using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using MediatR;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using System.Text.Json.Serialization;

namespace MetroMiles.ApplicationLayer.Features.Auths.Queries.GetSessionHistory;

public class GetSessionHistoryQuery : IRequest<OperationDataResult<GetListResponse<SessionHistoryItemDto>>>
{
    // Set by the controller from the caller's own JWT claims.
    [JsonIgnore]
    public Guid UserId { get; set; }

    public required PageRequest PageRequest { get; set; }

    public class GetSessionHistoryQueryHandler(IRefreshTokenRepository refreshTokenRepository)
        : IRequestHandler<GetSessionHistoryQuery, OperationDataResult<GetListResponse<SessionHistoryItemDto>>>
    {
        public async Task<OperationDataResult<GetListResponse<SessionHistoryItemDto>>> Handle(GetSessionHistoryQuery request, CancellationToken cancellationToken)
        {
            var sessions = await refreshTokenRepository.GetListAsync(
                predicate: rt => rt.UserId == request.UserId,
                orderBy: q => q.OrderByDescending(rt => rt.CreatedDate),
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken);

            GetListResponse<SessionHistoryItemDto> response = new()
            {
                Items = [.. sessions.Items.Select(ToDto)],
                Index = sessions.Index,
                Size = sessions.Size,
                Count = sessions.Count,
                Pages = sessions.Pages,
                HasPrevious = sessions.HasPrevious,
                HasNext = sessions.HasNext,
            };

            return Result.Success(response);
        }

        private static SessionHistoryItemDto ToDto(RefreshToken refreshToken) => new()
        {
            Id = refreshToken.Id,
            CreatedDate = refreshToken.CreatedDate,
            CreatedByIp = refreshToken.CreatedByIp,
            Expires = refreshToken.Expires,
            Revoked = refreshToken.Revoked,
            RevokedByIp = refreshToken.RevokedByIp,
            IsActive = refreshToken.Revoked is null && refreshToken.Expires > DateTime.UtcNow,
        };
    }
}
