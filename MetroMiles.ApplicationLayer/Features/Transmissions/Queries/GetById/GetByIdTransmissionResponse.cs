namespace MetroMiles.ApplicationLayer.Features.Transmissions.Queries.GetById;

public class GetByIdTransmissionResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
}
