using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(u => u.FirstName).NotEmpty().WithMessage("First name is required.")
            .MinimumLength(2).WithMessage("First name must be at least {MinLength} characters long.");
        RuleFor(u => u.LastName).NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(2).WithMessage("Last name must be at least {MinLength} characters long.");
        RuleFor(u => u.Email).NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");
        RuleFor(u => u.Password).NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least {MinLength} characters long.");
    }
}
