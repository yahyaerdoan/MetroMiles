namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Create;

public class CreatedTransmissionResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }
}
