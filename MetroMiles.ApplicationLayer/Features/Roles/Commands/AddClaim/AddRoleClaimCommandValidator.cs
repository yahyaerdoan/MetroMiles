using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Roles.Commands.AddClaim;

public class AddRoleClaimCommandValidator : AbstractValidator<AddRoleClaimCommand>
{
    public AddRoleClaimCommandValidator()
    {
        RuleFor(r => r.RoleId).NotEmpty().WithMessage("RoleId is required.");
        RuleFor(r => r.ClaimValue).NotEmpty().WithMessage("ClaimValue is required.");
    }
}
