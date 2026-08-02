namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Update;

public class UpdatedFuelResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public byte[]? RowVersion { get; set; }
}
