using Hateoas;

namespace MetroMiles.ApplicationLayer.Features.Users.Queries.GetById;

public class GetByIdUserResponse : LinkedResponse
{
    public Guid Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public DateTimeOffset? UpdatedDate { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }

    public byte[]? RowVersion { get; set; }
}
