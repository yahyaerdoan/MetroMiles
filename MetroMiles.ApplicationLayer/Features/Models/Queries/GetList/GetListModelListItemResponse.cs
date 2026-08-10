using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Models.Queries.GetList;

public class GetListModelListItemResponse : LinkedResponse
{
    public Guid Id { get; set; }

    public required string BrandName { get; set; }

    public required string FuelName { get; set; }

    public required string TransmissionName { get; set; }

    public required string Name { get; set; }

    public decimal DailyPrice { get; set; }

    public required string ImageUrl { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }
}
