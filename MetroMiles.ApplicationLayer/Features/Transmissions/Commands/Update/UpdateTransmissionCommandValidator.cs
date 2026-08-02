using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Update;

public class UpdateTransmissionCommandValidator : AbstractValidator<UpdateTransmissionCommand>
{
    public UpdateTransmissionCommandValidator()
    {
        RuleFor(t => t.Id).NotEmpty();
        RuleFor(t => t.Name).NotEmpty().MinimumLength(2);
    }
}
