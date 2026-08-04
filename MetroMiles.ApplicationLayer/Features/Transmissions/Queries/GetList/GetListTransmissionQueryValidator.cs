using FluentValidation;
using MetroMiles.ApplicationLayer.Extensions.Validations;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Queries.GetList;

public class GetListTransmissionQueryValidator : AbstractValidator<GetListTransmissionQuery>
{
    public GetListTransmissionQueryValidator()
    {
        RuleFor(q => q.PageRequest).SetValidator(new PageRequestValidator());
    }
}
