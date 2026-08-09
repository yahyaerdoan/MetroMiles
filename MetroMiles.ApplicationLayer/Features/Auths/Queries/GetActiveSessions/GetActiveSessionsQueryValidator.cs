using FluentValidation;
using MetroMiles.ApplicationLayer.Extensions.Validations;

namespace MetroMiles.ApplicationLayer.Features.Auths.Queries.GetActiveSessions;

public class GetActiveSessionsQueryValidator : AbstractValidator<GetActiveSessionsQuery>
{
    public GetActiveSessionsQueryValidator()
    {
        RuleFor(q => q.PageRequest).SetValidator(new PageRequestValidator());
    }
}
