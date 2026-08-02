using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Create;

public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
{
    public CreateCarCommandValidator()
    {
        RuleFor(c => c.ModelId).NotEmpty();
        RuleFor(c => c.Plate).NotEmpty();
        RuleFor(c => c.Kilometer).GreaterThanOrEqualTo(0);
        RuleFor(c => c.Mile).GreaterThanOrEqualTo(0);
        RuleFor(c => c.ModelYear).GreaterThan((short)1900);
        RuleFor(c => c.MinFindexScore).GreaterThanOrEqualTo((short)0);
        RuleFor(c => c.Status).IsInEnum();
    }
}
