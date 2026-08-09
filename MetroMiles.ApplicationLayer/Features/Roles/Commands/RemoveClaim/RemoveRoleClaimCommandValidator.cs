using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.RemoveClaim;

public class RemoveRoleClaimCommandValidator : AbstractValidator<RemoveRoleClaimCommand>
{
    public RemoveRoleClaimCommandValidator()
    {
        RuleFor(r => r.RoleId).NotEmpty().WithMessage("RoleId is required.");
        RuleFor(r => r.ClaimValue).NotEmpty().WithMessage("ClaimValue is required.");
    }
}
