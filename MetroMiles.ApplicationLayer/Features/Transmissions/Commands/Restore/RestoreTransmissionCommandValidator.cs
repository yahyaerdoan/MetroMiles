using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Restore;

public class RestoreTransmissionCommandValidator : AbstractValidator<RestoreTransmissionCommand>
{
    public RestoreTransmissionCommandValidator()
    {
        RuleFor(t => t.Id).NotEmpty().WithMessage("Id is required.");
    }
}
