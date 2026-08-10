using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Restore;

public class RestoredFuelResponse : LinkedResponse
{
    public Guid Id { get; set; }
}
