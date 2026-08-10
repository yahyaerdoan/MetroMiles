using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Delete;

public class DeletedTransmissionResponse : LinkedResponse
{
    public Guid Id { get; set; }
}
