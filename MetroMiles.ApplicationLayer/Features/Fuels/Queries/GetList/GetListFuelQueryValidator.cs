using FluentValidation;
using MetroMiles.ApplicationLayer.Extensions.Validations;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetList;

public class GetListFuelQueryValidator : AbstractValidator<GetListFuelQuery>
{
    public GetListFuelQueryValidator()
    {
        RuleFor(q => q.PageRequest).SetValidator(new PageRequestValidator());
    }
}
