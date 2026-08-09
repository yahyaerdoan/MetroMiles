using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.Create;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(r => r.Name).NotEmpty().WithMessage("Name is required.")
            .MaximumLength(256).WithMessage("Name must be at most {MaxLength} characters long.");
    }
}
