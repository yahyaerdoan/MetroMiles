using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Models.Commands.Update;

public class UpdateModelCommandValidator : AbstractValidator<UpdateModelCommand>
{
    public UpdateModelCommandValidator()
    {
        RuleFor(m => m.Id).NotEmpty();
        RuleFor(m => m.BrandId).NotEmpty();
        RuleFor(m => m.FuelId).NotEmpty();
        RuleFor(m => m.TransmissionId).NotEmpty();
        RuleFor(m => m.Name).NotEmpty().MinimumLength(2);
        RuleFor(m => m.DailyPrice).GreaterThan(0);
        RuleFor(m => m.ImageUrl).NotEmpty();
    }
}
