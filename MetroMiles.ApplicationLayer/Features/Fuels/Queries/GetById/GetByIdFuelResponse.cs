using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetById;

public class GetByIdFuelResponse : LinkedResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }

    public byte[]? RowVersion { get; set; }
}
