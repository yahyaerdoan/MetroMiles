using Core.PersistenceLayer.Repositories.Entities;

namespace MetroMiles.DomainLayer.Entities;

public class RefreshToken : Entity<Guid>
{
    public Guid UserId { get; set; }

    public string Token { get; set; }

    public DateTime Expires { get; set; }

    public string CreatedByIp { get; set; }

    public DateTime? Revoked { get; set; }

    public string? RevokedByIp { get; set; }

    public string? ReplacedByToken { get; set; }

    public virtual User User { get; set; } = null!;

    public RefreshToken()
    {
        Token = string.Empty;
        CreatedByIp = string.Empty;
    }

    public RefreshToken(Guid userId, string token, DateTime expires, string createdByIp)
    {
        UserId = userId;
        Token = token;
        Expires = expires;
        CreatedByIp = createdByIp;
    }
}
