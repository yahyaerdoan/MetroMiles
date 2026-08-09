using System.Text.Json.Serialization;
using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.ApplicationLayer.Pipelines.Loggings.Abstractions;
using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Pagings.Extensions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Users.Constants;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Users.Queries.GetList;

// No ICacheAddRequest here (unlike GetListBrandQuery) — CreateUserCommand/UpdateUserCommand don't
// implement ICacheRemoveRequest, so caching this list would go stale after every create/update.
public class GetListUserQuery : IRequest<OperationDataResult<GetListResponse<GetListUserListItemDto>>>, ILogAddRequest, ISecureAddRequest
{
    public required PageRequest PageRequest { get; set; }

    [JsonIgnore]
    public string[] Roles => [UsersOperationClaims.Admin, UsersOperationClaims.Read];

    public class GetListUserQueryHandler(UserManager<User> userManager, IMapper mapper) : IRequestHandler<GetListUserQuery, OperationDataResult<GetListResponse<GetListUserListItemDto>>>
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetListResponse<GetListUserListItemDto>>> Handle(GetListUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _userManager.Users.OrderBy(u => u.UserName).ToPaginateAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken);
            var response = _mapper.Map<GetListResponse<GetListUserListItemDto>>(users);
            return Result.Success(response);
        }
    }
}
