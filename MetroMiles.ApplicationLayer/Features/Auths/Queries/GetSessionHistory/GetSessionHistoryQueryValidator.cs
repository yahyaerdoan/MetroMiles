using FluentValidation;
using MetroMiles.ApplicationLayer.Extensions.Validations;

namespace MetroMiles.ApplicationLayer.Features.Auths.Queries.GetSessionHistory;

public class GetSessionHistoryQueryValidator : AbstractValidator<GetSessionHistoryQuery>
{
    public GetSessionHistoryQueryValidator()
    {
        RuleFor(q => q.PageRequest).SetValidator(new PageRequestValidator());
    }
}
