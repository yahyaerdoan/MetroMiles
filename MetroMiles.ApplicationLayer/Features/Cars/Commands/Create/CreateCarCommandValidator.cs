using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Create;

public class CreateCarCommandValidator : AbstractValidator<CreateCarCommand>
{
    public CreateCarCommandValidator()
    {
        RuleFor(c => c.ModelId).NotEmpty().WithMessage("Model is required.");
        RuleFor(c => c.Plate).NotEmpty().WithMessage("Plate is required.");
        RuleFor(c => c.Kilometer).GreaterThanOrEqualTo(0).WithMessage("Kilometer must be {ComparisonValue} or greater.");
        RuleFor(c => c.Mile).GreaterThanOrEqualTo(0).WithMessage("Mile must be {ComparisonValue} or greater.");
        RuleFor(c => c.ModelYear).GreaterThan((short)1900).WithMessage("Model year must be after {ComparisonValue}.")
            .LessThanOrEqualTo((short)(DateTime.UtcNow.Year + 1)).WithMessage("Model year must be {ComparisonValue} or earlier.");
        RuleFor(c => c.MinFindexScore).GreaterThanOrEqualTo((short)0).WithMessage("Minimum findex score must be {ComparisonValue} or greater.");
        RuleFor(c => c.Status).IsInEnum().WithMessage("Status must be a valid value.");
    }
}
