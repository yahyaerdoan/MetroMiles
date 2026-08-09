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

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Delete;

public class DeleteUserCommand : SecuredCommand<Guid, DeletedUserResponse>
{
    [JsonIgnore]
    public override string[] Roles => UsersOperationClaims.DeleteRoles;

    public class DeleteUserCommandHandler(UserManager<User> userManager, IMapper mapper) : IRequestHandler<DeleteUserCommand, OperationDataResult<DeletedUserResponse>>
    {
        public async Task<OperationDataResult<DeletedUserResponse>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await userManager.FindByIdAsync(request.Id.ToString()!);
            var existenceCheck = UserBusinessRules.UserShouldBeExistsWhenSelected(existingUser);
            if (!existenceCheck.IsSuccessful)
            {
                return existenceCheck.ToErrorDataResult<DeletedUserResponse>();
            }

            existenceCheck.Data.DeletedDate = DateTimeOffset.UtcNow;
            var deleteResult = await userManager.UpdateAsync(existenceCheck.Data);
            if (!deleteResult.Succeeded)
            {
                return Result.BadRequest<DeletedUserResponse>(string.Join(" ", deleteResult.Errors.Select(e => e.Description)));
            }

            return Result.Success(mapper.Map<DeletedUserResponse>(existenceCheck.Data));
        }
    }
}
