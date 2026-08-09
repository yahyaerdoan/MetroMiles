using Core.SecurityLayer.Identity;

namespace MetroMiles.DomainLayer.Entities;

public class User : SequentialGuidIdentityUser
{
    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? UpdatedDate { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }

    public byte[]? RowVersion { get; set; }
}
