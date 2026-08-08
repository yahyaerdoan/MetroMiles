using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.RevokeToken;

public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>
{
    public RevokeTokenCommandValidator()
    {
        RuleFor(r => r.Token).NotEmpty().WithMessage("Token is required.");
    }
}
