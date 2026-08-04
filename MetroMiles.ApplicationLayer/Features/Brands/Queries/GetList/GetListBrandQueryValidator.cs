using FluentValidation;
using MetroMiles.ApplicationLayer.Extensions.Validations;

namespace MetroMiles.ApplicationLayer.Features.Brands.Queries.GetList;

public class GetListBrandQueryValidator : AbstractValidator<GetListBrandQuery>
{
    public GetListBrandQueryValidator()
    {
        RuleFor(q => q.PageRequest).SetValidator(new PageRequestValidator());
    }
}
