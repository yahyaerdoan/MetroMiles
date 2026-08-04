using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetById;

public class GetByIdFuelQueryValidator : AbstractValidator<GetByIdFuelQuery>
{
    public GetByIdFuelQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty();
    }
}
