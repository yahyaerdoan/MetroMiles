using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Update;

public class UpdateFuelCommandValidator : AbstractValidator<UpdateFuelCommand>
{
    public UpdateFuelCommandValidator()
    {
        RuleFor(f => f.Id).NotEmpty();
        RuleFor(f => f.Name).NotEmpty().MinimumLength(2);
    }
}
