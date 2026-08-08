using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Cars.Queries.GetById;

public class GetByIdCarQueryValidator : AbstractValidator<GetByIdCarQuery>
{
    public GetByIdCarQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Id is required.");
    }
}
