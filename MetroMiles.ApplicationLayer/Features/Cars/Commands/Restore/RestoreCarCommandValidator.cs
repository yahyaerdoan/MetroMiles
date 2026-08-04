using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Restore;

public class RestoreCarCommandValidator : AbstractValidator<RestoreCarCommand>
{
    public RestoreCarCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
