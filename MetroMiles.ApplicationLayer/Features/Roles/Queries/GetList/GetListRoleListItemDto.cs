namespace MetroMiles.ApplicationLayer.Features.Roles.Queries.GetList;

public class GetListRoleListItemDto
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required List<string> Claims { get; set; }
}
