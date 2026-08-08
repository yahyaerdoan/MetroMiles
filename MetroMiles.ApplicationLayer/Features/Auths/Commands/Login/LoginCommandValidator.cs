using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(l => l.Email).NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");
        RuleFor(l => l.Password).NotEmpty().WithMessage("Password is required.");
    }
}
