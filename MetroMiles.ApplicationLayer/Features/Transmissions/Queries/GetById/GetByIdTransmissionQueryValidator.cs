using FluentValidation;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Queries.GetById;

public class GetByIdTransmissionQueryValidator : AbstractValidator<GetByIdTransmissionQuery>
{
    public GetByIdTransmissionQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty();
    }
}
