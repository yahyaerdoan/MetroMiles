using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Update;

public class UpdateTransmissionCommandValidator : AbstractValidator<UpdateTransmissionCommand>
{
    public UpdateTransmissionCommandValidator()
    {
        RuleFor(t => t.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(t => t.Name).NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name must be at least {MinLength} characters long.");
    }
}
