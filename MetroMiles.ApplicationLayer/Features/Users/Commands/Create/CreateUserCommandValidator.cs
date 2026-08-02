using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Create;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(u => u.FirstName).NotEmpty().MinimumLength(2);
        RuleFor(u => u.LastName).NotEmpty().MinimumLength(2);
        RuleFor(u => u.Email).NotEmpty().EmailAddress();
        RuleFor(u => u.Password).NotEmpty().MinimumLength(6);
    }
}
