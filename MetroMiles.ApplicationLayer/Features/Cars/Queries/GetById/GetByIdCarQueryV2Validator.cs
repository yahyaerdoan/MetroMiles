using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Cars.Queries.GetById;

public class GetByIdCarQueryV2Validator : AbstractValidator<GetByIdCarQueryV2>
{
    public GetByIdCarQueryV2Validator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Id is required.");
    }
}
