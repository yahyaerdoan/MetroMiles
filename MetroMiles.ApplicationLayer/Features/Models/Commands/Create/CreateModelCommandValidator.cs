using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Models.Commands.Create;

public class CreateModelCommandValidator : AbstractValidator<CreateModelCommand>
{
    public CreateModelCommandValidator()
    {
        RuleFor(m => m.BrandId).NotEmpty();
        RuleFor(m => m.FuelId).NotEmpty();
        RuleFor(m => m.TransmissionId).NotEmpty();
        RuleFor(m => m.Name).NotEmpty().MinimumLength(2);
        RuleFor(m => m.DailyPrice).GreaterThan(0);
        RuleFor(m => m.ImageUrl).NotEmpty();
    }
}
