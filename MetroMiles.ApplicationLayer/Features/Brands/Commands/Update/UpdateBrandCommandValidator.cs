using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Update;

public class UpdateBrandCommandValidator : AbstractValidator<UpdateBrandCommand>
{
    public UpdateBrandCommandValidator()
    {
        RuleFor(b => b.Id).NotEmpty();
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2);
    }
}
