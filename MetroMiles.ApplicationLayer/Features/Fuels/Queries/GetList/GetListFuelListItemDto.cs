using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetList;

public class GetListFuelListItemDto : LinkedResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }
}
