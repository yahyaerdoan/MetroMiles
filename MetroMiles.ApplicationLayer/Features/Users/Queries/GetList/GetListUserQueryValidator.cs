using FluentValidation;
using MetroMiles.ApplicationLayer.Extensions.Validations;

namespace MetroMiles.ApplicationLayer.Features.Users.Queries.GetList;

public class GetListUserQueryValidator : AbstractValidator<GetListUserQuery>
{
    public GetListUserQueryValidator()
    {
        RuleFor(q => q.PageRequest).SetValidator(new PageRequestValidator());
    }
}
