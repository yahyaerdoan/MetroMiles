using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Users.Constants;
using MetroMiles.ApplicationLayer.Features.Users.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;

using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Users.Queries.GetById;

public class GetByIdUserQuery : IRequest<OperationDataResult<GetByIdUserResponse>>, ISecureAddRequest
{
    public int Id { get; set; }
    public string[] Roles => [UsersOperationClaims.Admin, UsersOperationClaims.Read];

    public class GetByIdUserQueryHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<GetByIdUserQuery, OperationDataResult<GetByIdUserResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<GetByIdUserResponse>> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(predicate: u => u.Id == request.Id, cancellationToken: cancellationToken);
            var existenceCheck = UserBusinessRules.UserShouldBeExistsWhenSelected(user);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<GetByIdUserResponse>();
            }

            return Result.Success(_mapper.Map<GetByIdUserResponse>(existenceCheck.Data));
        }
    }
}
