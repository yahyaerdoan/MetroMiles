using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Users.Commands.Delete;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(u => u.Id).NotEmpty().WithMessage("Id is required.");
    }
}
