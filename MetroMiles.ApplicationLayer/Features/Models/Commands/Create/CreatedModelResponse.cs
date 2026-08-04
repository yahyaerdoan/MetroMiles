namespace MetroMiles.ApplicationLayer.Features.Models.Commands.Create;

public class CreatedModelResponse
{
    public Guid Id { get; set; }
    public Guid BrandId { get; set; }
    public Guid FuelId { get; set; }
    public Guid TransmissionId { get; set; }
    public required string Name { get; set; }
    public decimal DailyPrice { get; set; }
    public required string ImageUrl { get; set; }
}
