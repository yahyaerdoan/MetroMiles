using MetroMiles.DomainLayer.Entities.Enums;

namespace MetroMiles.ApplicationLayer.Features.Cars.Commands.Update;

public class UpdatedCarResponse
{
    public Guid Id { get; set; }
    public Guid ModelId { get; set; }
    public int Kilometer { get; set; }
    public int Mile { get; set; }
    public short ModelYear { get; set; }
    public required string Plate { get; set; }
    public short MinFindexScore { get; set; }
    public CarStatus Status { get; set; }
    public byte[]? RowVersion { get; set; }
}
