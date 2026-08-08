using Core.ApplicationLayer.Requests.Page;
using FluentValidation;

namespace MetroMiles.ApplicationLayer.Extensions.Validations;

// Shared via SetValidator() from each GetListXQuery's own validator — PageRequest itself is never
// sent as a MediatR request, so it's not picked up by the validation pipeline on its own.
public class PageRequestValidator : AbstractValidator<PageRequest>
{
    public const int MaxPageSize = 100;

    public PageRequestValidator()
    {
        RuleFor(p => p.PageIndex).GreaterThanOrEqualTo(0).WithMessage("Page index must be {ComparisonValue} or greater.")
            .When(p => p.PageIndex.HasValue);
        RuleFor(p => p.PageSize).InclusiveBetween(1, MaxPageSize).WithMessage("Page size must be between {From} and {To}.")
            .When(p => p.PageSize.HasValue);
    }
}
