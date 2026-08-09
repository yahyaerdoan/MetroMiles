using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.RevokeSession;

public class RevokeSessionCommandValidator : AbstractValidator<RevokeSessionCommand>
{
    public RevokeSessionCommandValidator()
    {
        RuleFor(c => c.SessionId).NotEmpty().WithMessage("SessionId is required.");
    }
}
