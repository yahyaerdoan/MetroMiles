using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Delete;

public class DeleteBrandCommandValidator : AbstractValidator<DeleteBrandCommand>
{
    public DeleteBrandCommandValidator()
    {
        RuleFor(b => b.Id).NotEmpty().WithMessage("Id is required.");
    }
}
