using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using Core.ApplicationLayer.Pipelines.Loggings.Abstractions;
using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Pagings.Paging;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Users.Constants;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Users.Queries.GetList;

// No ICacheAddRequest here (unlike GetListBrandQuery) — CreateUserCommand/UpdateUserCommand don't
// implement ICacheRemoveRequest, so caching this list would go stale after every create/update.
public class GetListUserQuery : IRequest<OperationDataResult<GetListResponse<GetListUserListItemDto>>>, ILogAddRequest, ISecureAddRequest
{
    public required PageRequest PageRequest { get; set; }
    public string[] Roles => [UsersOperationClaims.Admin, UsersOperationClaims.Read];

    public class GetListUserQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetListUserQuery, OperationDataResult<GetListResponse<GetListUserListItemDto>>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetListResponse<GetListUserListItemDto>>> Handle(GetListUserQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken);
            var response = _mapper.Map<GetListResponse<GetListUserListItemDto>>(users);
            return Result.Success(response);
        }
    }
}
