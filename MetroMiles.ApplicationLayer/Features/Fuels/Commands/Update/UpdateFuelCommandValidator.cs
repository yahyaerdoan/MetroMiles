using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Update;

public class UpdateFuelCommandValidator : AbstractValidator<UpdateFuelCommand>
{
    public UpdateFuelCommandValidator()
    {
        RuleFor(f => f.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(f => f.Name).NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name must be at least {MinLength} characters long.");
    }
}
