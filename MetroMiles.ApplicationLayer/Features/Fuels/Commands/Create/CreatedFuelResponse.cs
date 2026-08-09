namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Create;

public class CreatedFuelResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }
}
