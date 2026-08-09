namespace MetroMiles.ApplicationLayer.Features.Users.Queries.GetList;

public class GetListUserListItemDto
{
    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }
}
