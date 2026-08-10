namespace MetroMiles.ApplicationLayer.Features.Auths.Queries.GetSessionHistory;

public class SessionHistoryItemResponse
{
    public Guid Id { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public DateTime Expires { get; set; }

    public required string CreatedByIp { get; set; }

    public DateTime? Revoked { get; set; }

    public string? RevokedByIp { get; set; }

    public bool IsActive { get; set; }
}
