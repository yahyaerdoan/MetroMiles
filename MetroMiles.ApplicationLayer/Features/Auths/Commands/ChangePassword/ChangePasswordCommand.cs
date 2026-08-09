using MediatR;
using MetroMiles.ApplicationLayer.Features.Users.Rules;
using MetroMiles.DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using ResultHandler.Core.Base;
using ResultHandler.Facade;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest<OperationResult>
{
    // Set by the controller from the caller's own JWT claims — never trust a client-supplied user id
    // here, or one authenticated user could change another user's password.
    public Guid UserId { get; set; }

    public string CurrentPassword { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;

    public class ChangePasswordCommandHandler(UserManager<User> userManager) : IRequestHandler<ChangePasswordCommand, OperationResult>
    {
        public async Task<OperationResult> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.UserId.ToString());
            var existenceCheck = UserBusinessRules.UserShouldBeExistsWhenSelected(user);
            if (!existenceCheck.IsSuccessful)
            {
                return (OperationResult)existenceCheck;
            }

            var changeResult = await userManager.ChangePasswordAsync(existenceCheck.Data, request.CurrentPassword, request.NewPassword);
            if (!changeResult.Succeeded)
            {
                return Result.BadRequest(string.Join(" ", changeResult.Errors.Select(e => e.Description)));
            }

            return Result.Success();
        }
    }
}
