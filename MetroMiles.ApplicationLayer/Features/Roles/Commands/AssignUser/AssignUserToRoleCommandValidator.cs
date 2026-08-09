using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.AssignUser;

public class AssignUserToRoleCommandValidator : AbstractValidator<AssignUserToRoleCommand>
{
    public AssignUserToRoleCommandValidator()
    {
        RuleFor(r => r.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(r => r.RoleName).NotEmpty().WithMessage("RoleName is required.");
    }
}
