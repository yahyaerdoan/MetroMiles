using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using MetroMiles.ApplicationLayer.Extensions.Requests;
using MetroMiles.ApplicationLayer.Features.Users.Constants;
using MetroMiles.ApplicationLayer.Features.Users.Rules;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;
using ResultHandler.Functional;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Update;

public class UpdateUserCommand : SecuredCommand<Guid, UpdatedUserResponse>
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public byte[]? RowVersion { get; set; }

    [JsonIgnore]
    public override string[] Roles => UsersOperationClaims.UpdateRoles;

    public class UpdateUserCommandHandler(UserManager<User> userManager, IMapper mapper, UserBusinessRules userBusinessRules)
        : IRequestHandler<UpdateUserCommand, OperationDataResult<UpdatedUserResponse>>
    {
        public async Task<OperationDataResult<UpdatedUserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await userManager.FindByIdAsync(request.Id.ToString()!);
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

            var emailCheck = await userBusinessRules.UserEmailShouldNotExistsWhenUpdate(existenceCheck.Data.Id, request.Email);
            if (!emailCheck.IsSuccessful)
            {
                return emailCheck.ToErrorDataResult<UpdatedUserResponse>();
            }

            var user = existenceCheck.Data;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.Email;
            user.UpdatedDate = DateTimeOffset.UtcNow;

            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return Result.BadRequest<UpdatedUserResponse>(string.Join(" ", updateResult.Errors.Select(e => e.Description)));
            }

            return Result.Success(mapper.Map<UpdatedUserResponse>(user));
        }
    }
}
