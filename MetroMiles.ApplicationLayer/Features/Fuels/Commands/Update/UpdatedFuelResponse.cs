using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Commands.Update;

public class UpdatedFuelResponse : LinkedResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public byte[]? RowVersion { get; set; }
}
