namespace MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Update;

public class UpdatedTransmissionResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public byte[]? RowVersion { get; set; }
}
