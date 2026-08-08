using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Restore;

public class RestoreUserCommandValidator : AbstractValidator<RestoreUserCommand>
{
    public RestoreUserCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithMessage("Id is required.");
    }
}
