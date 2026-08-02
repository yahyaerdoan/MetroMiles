using FluentValidation;
using MetroMiles.ApplicationLayer.Extensions.Validations;

namespace MetroMiles.ApplicationLayer.Features.Cars.Queries.GetList;

public class GetListCarQueryValidator : AbstractValidator<GetListCarQuery>
{
    public GetListCarQueryValidator()
    {
        RuleFor(q => q.PageRequest).SetValidator(new PageRequestValidator());
    }
}
