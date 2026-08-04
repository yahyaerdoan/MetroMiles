using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Delete;

public class DeleteFuelCommandValidator : AbstractValidator<DeleteFuelCommand>
{
    public DeleteFuelCommandValidator()
    {
        RuleFor(f => f.Id).NotEmpty();
    }
}
