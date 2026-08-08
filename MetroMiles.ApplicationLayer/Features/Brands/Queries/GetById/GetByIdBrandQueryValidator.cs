using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Brands.Queries.GetById;

public class GetByIdBrandQueryValidator : AbstractValidator<GetByIdBrandQuery>
{
    public GetByIdBrandQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Id is required.");
    }
}
