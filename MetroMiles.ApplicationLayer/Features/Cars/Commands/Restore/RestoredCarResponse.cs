using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Restore;

public class RestoredCarResponse : LinkedResponse
{
    public Guid Id { get; set; }
}
