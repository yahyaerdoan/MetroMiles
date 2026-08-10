using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Brands.Queries.GetList;

public class GetListBrandListItemDto : LinkedResponse
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }
}
