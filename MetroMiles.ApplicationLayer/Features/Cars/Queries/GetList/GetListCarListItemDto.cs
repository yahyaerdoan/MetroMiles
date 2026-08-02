using MetroMiles.DomainLayer.Entities.Enums;

namespace MetroMiles.ApplicationLayer.Features.Cars.Queries.GetList;

public class GetListCarListItemDto
{
    public Guid Id { get; set; }
    public required string ModelName { get; set; }
    public required string BrandName { get; set; }
    public int Kilometer { get; set; }
    public short ModelYear { get; set; }
    public required string Plate { get; set; }
    public CarStatus Status { get; set; }
}
