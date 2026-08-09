namespace MetroMiles.ApplicationLayer.Features.Transmissions.Queries.GetList;

public class GetListTransmissionListItemDto
{
    public Guid Id { get; set; }

    public required string Name { get; set; }
}
