using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Users.Constants;
using MetroMiles.ApplicationLayer.Features.Users.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Restore;

public class RestoreUserCommand : IRequest<OperationDataResult<RestoredUserResponse>>, ISecureAddRequest
{
    public int? Id { get; set; }
    public string[] Roles => [UsersOperationClaims.Admin, UsersOperationClaims.Write, UsersOperationClaims.Update];

    public class RestoreUserCommandHandler(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules) : IRequestHandler<RestoreUserCommand, OperationDataResult<RestoredUserResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly UserBusinessRules _userBusinessRules = userBusinessRules;

        public async Task<OperationDataResult<RestoredUserResponse>> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetAsync(predicate: u => u.Id == request.Id, withDeleted: true, cancellationToken: cancellationToken);
            var existenceCheck = UserBusinessRules.UserShouldBeDeletedWhenRestored(existingUser);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<RestoredUserResponse>();
            }

            var emailCheck = await _userBusinessRules.UserEmailShouldNotExistsWhenUpdate(existenceCheck.Data.Id, existenceCheck.Data.Email);
            if (!emailCheck.IsSuccessful)
            {
                return emailCheck.ToErrorDataResult<RestoredUserResponse>();
            }

            existenceCheck.Data.DeletedDate = null;
            await _userRepository.UpdateAsync(existenceCheck.Data);
            return Result.Success(_mapper.Map<RestoredUserResponse>(existenceCheck.Data));
        }
    }
}
