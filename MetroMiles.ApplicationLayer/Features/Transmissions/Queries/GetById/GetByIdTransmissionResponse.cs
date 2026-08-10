using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Queries.GetById;

public class GetByIdTransmissionResponse : LinkedResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }

    public byte[]? RowVersion { get; set; }
}
