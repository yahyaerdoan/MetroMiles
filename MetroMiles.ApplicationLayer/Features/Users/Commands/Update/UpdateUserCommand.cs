using AutoMapper;
using Core.ApplicationLayer.Pipelines.Authorizations.Abstractions;
using MediatR;
using MetroMiles.ApplicationLayer.Features.Users.Constants;
using MetroMiles.ApplicationLayer.Features.Users.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Update;

public class UpdateUserCommand : IRequest<OperationDataResult<UpdatedUserResponse>>, ISecureAddRequest
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public byte[]? RowVersion { get; set; }
    public string[] Roles => [UsersOperationClaims.Admin, UsersOperationClaims.Write, UsersOperationClaims.Update];

    public class UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules)
        : IRequestHandler<UpdateUserCommand, OperationDataResult<UpdatedUserResponse>>
    {
        public async Task<OperationDataResult<UpdatedUserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await userRepository.GetAsync(predicate: u => u.Id == request.Id, cancellationToken: cancellationToken);
            var existenceCheck = UserBusinessRules.UserShouldBeExistsWhenSelected(existingUser);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<UpdatedUserResponse>();
            }

            var versionCheck = UserBusinessRules.UserRowVersionShouldMatchWhenUpdated(existenceCheck.Data, request.RowVersion);
            if (!versionCheck.IsSuccessful)
            {
                return versionCheck.ToErrorDataResult<UpdatedUserResponse>();
            }

            var emailCheck = await userBusinessRules.UserEmailShouldNotExistsWhenUpdate(request.Id, request.Email);
            if (!emailCheck.IsSuccessful)
            {
                return emailCheck.ToErrorDataResult<UpdatedUserResponse>();
            }

            var user = mapper.Map(request, existenceCheck.Data);
            await userRepository.UpdateAsync(user);

            return Result.Success(mapper.Map<UpdatedUserResponse>(user));
        }
    }
}
