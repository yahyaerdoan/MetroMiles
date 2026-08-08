using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Delete;

public class DeleteTransmissionCommandValidator : AbstractValidator<DeleteTransmissionCommand>
{
    public DeleteTransmissionCommandValidator()
    {
        RuleFor(t => t.Id).NotEmpty().WithMessage("Id is required.");
    }
}
