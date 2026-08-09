using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Extensions.Requests;
using MetroMiles.ApplicationLayer.Features.Users.Constants;
using MetroMiles.ApplicationLayer.Features.Users.Rules;
using MetroMiles.ApplicationLayer.Services.Repositories;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Restore;

public class RestoreUserCommand : SecuredCommand<Guid, RestoredUserResponse>
{
    [JsonIgnore]
    public override string[] Roles => UsersOperationClaims.UpdateRoles;

    public class RestoreUserCommandHandler(UserManager<User> userManager, IUserQueryRepository userQueryRepository, IMapper mapper, UserBusinessRules userBusinessRules)
        : IRequestHandler<RestoreUserCommand, OperationDataResult<RestoredUserResponse>>
    {
        public async Task<OperationDataResult<RestoredUserResponse>> Handle(RestoreUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await userQueryRepository.GetByIdWithDeletedAsync(request.Id, cancellationToken);
            var existenceCheck = UserBusinessRules.UserShouldBeDeletedWhenRestored(existingUser);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<RestoredUserResponse>();
            }

            var emailCheck = await userBusinessRules.UserEmailShouldNotExistsWhenUpdate(existenceCheck.Data.Id, existenceCheck.Data.Email!);
            if (!emailCheck.IsSuccessful)
            {
                return emailCheck.ToErrorDataResult<RestoredUserResponse>();
            }

            existenceCheck.Data.DeletedDate = null;
            var updateResult = await userManager.UpdateAsync(existenceCheck.Data);
            if (!updateResult.Succeeded)
            {
                return Result.BadRequest<RestoredUserResponse>(string.Join(" ", updateResult.Errors.Select(e => e.Description)));
            }

            return Result.Success(mapper.Map<RestoredUserResponse>(existenceCheck.Data));
        }
    }
}
