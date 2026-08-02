namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Create;

public class CreatedBrandResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
}
