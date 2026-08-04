using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.RefreshToken;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(r => r.Token).NotEmpty();
    }
}
