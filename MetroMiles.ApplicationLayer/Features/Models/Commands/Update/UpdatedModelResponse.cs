namespace MetroMiles.ApplicationLayer.Features.Models.Commands.Update;

public class UpdatedModelResponse
{
    public Guid Id { get; set; }
    public Guid BrandId { get; set; }
    public Guid FuelId { get; set; }
    public Guid TransmissionId { get; set; }
    public required string Name { get; set; }
    public decimal DailyPrice { get; set; }
    public required string ImageUrl { get; set; }
    public byte[]? RowVersion { get; set; }
}
