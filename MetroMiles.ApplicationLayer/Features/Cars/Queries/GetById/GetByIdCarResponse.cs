using Hateoas;
using MetroMiles.DomainLayer.Entities.Enums;

namespace MetroMiles.ApplicationLayer.Features.Cars.Queries.GetById;

public class GetByIdCarResponse : LinkedResponse
{
    public Guid Id { get; set; }

    public Guid ModelId { get; set; }

    public required string ModelName { get; set; }

    public required string BrandName { get; set; }

    public int Kilometer { get; set; }

    public int Mile { get; set; }

    public short ModelYear { get; set; }

    public required string Plate { get; set; }

    public short MinFindexScore { get; set; }

    public CarStatus Status { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }

    public byte[]? RowVersion { get; set; }

    public required string MileageCategory { get; set; }
}
