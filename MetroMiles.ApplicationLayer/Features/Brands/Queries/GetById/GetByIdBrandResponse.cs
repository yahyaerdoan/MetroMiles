namespace MetroMiles.ApplicationLayer.Features.Brands.Queries.GetById;

public class GetByIdBrandResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    public DateTimeOffset? DeletedDate { get; set; }
    public byte[]? RowVersion { get; set; }
}
