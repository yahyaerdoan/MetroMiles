using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(c => c.CurrentPassword).NotEmpty().WithMessage("Current password is required.");
        RuleFor(c => c.NewPassword).NotEmpty().WithMessage("New password is required.")
            .MinimumLength(6).WithMessage("New password must be at least {MinLength} characters long.")
            .NotEqual(c => c.CurrentPassword).WithMessage("New password must be different from the current password.");
    }
}
