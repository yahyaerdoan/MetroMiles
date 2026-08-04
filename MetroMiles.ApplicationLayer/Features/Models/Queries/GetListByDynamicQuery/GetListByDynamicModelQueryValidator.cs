using FluentValidation;
using MetroMiles.ApplicationLayer.Extensions.Validations;

namespace MetroMiles.ApplicationLayer.Features.Models.Queries.GetListByDynamicQuery;

public class GetListByDynamicModelQueryValidator : AbstractValidator<GetListByDynamicModelQuery>
{
    public GetListByDynamicModelQueryValidator()
    {
        RuleFor(q => q.PageRequest).SetValidator(new PageRequestValidator());
    }
}
