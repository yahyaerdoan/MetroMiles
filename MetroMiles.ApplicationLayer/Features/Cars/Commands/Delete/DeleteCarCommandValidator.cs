using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Delete;

public class DeleteCarCommandValidator : AbstractValidator<DeleteCarCommand>
{
    public DeleteCarCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
