using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Create;

public class CreateFuelCommandValidator : AbstractValidator<CreateFuelCommand>
{
    public CreateFuelCommandValidator()
    {
        RuleFor(f => f.Name).NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name must be at least {MinLength} characters long.");
    }
}
