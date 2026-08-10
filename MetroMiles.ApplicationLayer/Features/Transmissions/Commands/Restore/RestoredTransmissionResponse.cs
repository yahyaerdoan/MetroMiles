using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Restore;

public class RestoredTransmissionResponse : LinkedResponse
{
    public Guid Id { get; set; }
}
