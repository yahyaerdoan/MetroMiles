using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Update;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(u => u.FirstName).NotEmpty().MinimumLength(2);
        RuleFor(u => u.LastName).NotEmpty().MinimumLength(2);
        RuleFor(u => u.Email).NotEmpty().EmailAddress();
    }
}
