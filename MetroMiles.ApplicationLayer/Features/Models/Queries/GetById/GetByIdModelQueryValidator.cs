using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Models.Queries.GetById;

public class GetByIdModelQueryValidator : AbstractValidator<GetByIdModelQuery>
{
    public GetByIdModelQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Id is required.");
    }
}
