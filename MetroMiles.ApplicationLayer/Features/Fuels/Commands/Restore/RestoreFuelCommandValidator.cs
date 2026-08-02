using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Restore;

public class RestoreFuelCommandValidator : AbstractValidator<RestoreFuelCommand>
{
    public RestoreFuelCommandValidator()
    {
        RuleFor(f => f.Id).NotEmpty();
    }
}
