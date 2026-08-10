using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Transmissions.Queries.GetList;

public class GetListTransmissionListItemDto : LinkedResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }
}
