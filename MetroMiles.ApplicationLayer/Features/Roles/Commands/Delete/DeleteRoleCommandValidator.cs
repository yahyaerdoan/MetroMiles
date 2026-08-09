using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.Delete;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty().WithMessage("Id is required.");
    }
}
