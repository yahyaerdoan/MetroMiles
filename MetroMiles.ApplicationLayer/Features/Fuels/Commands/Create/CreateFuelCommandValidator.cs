using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Create;

public class CreateFuelCommandValidator : AbstractValidator<CreateFuelCommand>
{
    public CreateFuelCommandValidator()
    {
        RuleFor(f => f.Name).NotEmpty().MinimumLength(2);
    }
}
