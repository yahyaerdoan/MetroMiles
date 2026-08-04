namespace MetroMiles.ApplicationLayer.Features.Brands.Commands.Update;

public class UpdatedBrandResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset UpdatedDate { get; set; }
    public byte[]? RowVersion { get; set; }
}
