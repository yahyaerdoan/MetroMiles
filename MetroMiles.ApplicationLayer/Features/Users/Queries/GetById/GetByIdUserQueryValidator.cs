using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Users.Queries.GetById;

public class GetByIdUserQueryValidator : AbstractValidator<GetByIdUserQuery>
{
    public GetByIdUserQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Id is required.");
    }
}
