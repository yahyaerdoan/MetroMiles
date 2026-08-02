using FluentValidation;
using MetroMiles.ApplicationLayer.Extensions.Validations;

namespace MetroMiles.ApplicationLayer.Features.Models.Queries.GetList;

public class GetListModelQueryValidator : AbstractValidator<GetListModelQuery>
{
    public GetListModelQueryValidator()
    {
        RuleFor(q => q.PageRequest).SetValidator(new PageRequestValidator());
    }
}
