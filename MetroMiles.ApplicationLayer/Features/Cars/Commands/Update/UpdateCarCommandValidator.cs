using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Update;

public class UpdateCarCommandValidator : AbstractValidator<UpdateCarCommand>
{
    public UpdateCarCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
        RuleFor(c => c.ModelId).NotEmpty();
        RuleFor(c => c.Plate).NotEmpty();
        RuleFor(c => c.Kilometer).GreaterThanOrEqualTo(0);
        RuleFor(c => c.Mile).GreaterThanOrEqualTo(0);
        RuleFor(c => c.ModelYear).GreaterThan((short)1900).LessThanOrEqualTo((short)(DateTime.UtcNow.Year + 1));
        RuleFor(c => c.MinFindexScore).GreaterThanOrEqualTo((short)0);
        RuleFor(c => c.Status).IsInEnum();
    }
}
