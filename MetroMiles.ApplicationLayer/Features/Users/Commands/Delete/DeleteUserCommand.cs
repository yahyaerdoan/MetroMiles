using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Users.Constants;
using MetroMiles.ApplicationLayer.Features.Users.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Delete;

public class DeleteUserCommand : IRequest<OperationDataResult<DeletedUserResponse>>, ISecureAddRequest
{
    public int? Id { get; set; }
    public string[] Roles => [UsersOperationClaims.Admin, UsersOperationClaims.Write, UsersOperationClaims.Delete];

    public class DeleteUserCommandHandler(IUserRepository userRepository, IMapper mapper) : IRequestHandler<DeleteUserCommand, OperationDataResult<DeletedUserResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<OperationDataResult<DeletedUserResponse>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetAsync(predicate: u => u.Id == request.Id, withDeleted: false, cancellationToken: cancellationToken);
            var existenceCheck = UserBusinessRules.UserShouldBeExistsWhenSelected(existingUser);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<DeletedUserResponse>();
            }

            await _userRepository.DeleteAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<DeletedUserResponse>(existenceCheck.Data));
        }
    }
}
