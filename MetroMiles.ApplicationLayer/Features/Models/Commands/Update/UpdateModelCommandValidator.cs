using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Models.Commands.Update;

public class UpdateModelCommandValidator : AbstractValidator<UpdateModelCommand>
{
    public UpdateModelCommandValidator()
    {
        RuleFor(m => m.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(m => m.BrandId).NotEmpty().WithMessage("Brand is required.");
        RuleFor(m => m.FuelId).NotEmpty().WithMessage("Fuel is required.");
        RuleFor(m => m.TransmissionId).NotEmpty().WithMessage("Transmission is required.");
        RuleFor(m => m.Name).NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name must be at least {MinLength} characters long.");
        RuleFor(m => m.DailyPrice).GreaterThan(0).WithMessage("Daily price must be greater than {ComparisonValue}.");
        RuleFor(m => m.ImageUrl).NotEmpty().WithMessage("Image URL is required.");
    }
}
