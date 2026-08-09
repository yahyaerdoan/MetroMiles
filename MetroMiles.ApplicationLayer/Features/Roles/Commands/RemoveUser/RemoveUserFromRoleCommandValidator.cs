using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.RemoveUser;

public class RemoveUserFromRoleCommandValidator : AbstractValidator<RemoveUserFromRoleCommand>
{
    public RemoveUserFromRoleCommandValidator()
    {
        RuleFor(r => r.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(r => r.RoleName).NotEmpty().WithMessage("RoleName is required.");
    }
}
