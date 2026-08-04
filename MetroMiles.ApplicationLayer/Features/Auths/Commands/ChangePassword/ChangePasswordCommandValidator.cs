using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(c => c.CurrentPassword).NotEmpty();
        RuleFor(c => c.NewPassword).NotEmpty().MinimumLength(6).NotEqual(c => c.CurrentPassword);
    }
}
