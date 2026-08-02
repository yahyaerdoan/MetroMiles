using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Create;

public class CreateTransmissionCommandValidator : AbstractValidator<CreateTransmissionCommand>
{
    public CreateTransmissionCommandValidator()
    {
        RuleFor(t => t.Name).NotEmpty().MinimumLength(2);
    }
}
