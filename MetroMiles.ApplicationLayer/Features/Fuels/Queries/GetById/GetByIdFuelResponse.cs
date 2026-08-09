namespace MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetById;

public class GetByIdFuelResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public byte[]? RowVersion { get; set; }
}
