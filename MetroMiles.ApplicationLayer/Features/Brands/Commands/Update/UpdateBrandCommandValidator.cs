using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Update;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(b => b.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(b => b.Name).NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name must be at least {MinLength} characters long.");
    }
}
