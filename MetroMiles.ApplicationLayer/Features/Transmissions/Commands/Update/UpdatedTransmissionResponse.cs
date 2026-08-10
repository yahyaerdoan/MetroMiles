using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Update;

public class UpdatedTransmissionResponse : LinkedResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public byte[]? RowVersion { get; set; }
}
