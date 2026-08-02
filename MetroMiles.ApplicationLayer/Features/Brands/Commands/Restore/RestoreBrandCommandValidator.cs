using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Restore;

public class RestoreBrandCommandValidator : AbstractValidator<RestoreBrandCommand>
{
    public RestoreBrandCommandValidator()
    {
        RuleFor(b => b.Id).NotEmpty();
    }
}
