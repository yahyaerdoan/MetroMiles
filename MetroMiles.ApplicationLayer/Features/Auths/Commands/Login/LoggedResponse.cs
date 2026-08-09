namespace MetroMiles.ApplicationLayer.Features.Auths.Commands.Login;

public class LoggedResponse
{
    public string Token { get; set; }

    public DateTime Expiration { get; set; }

    public string RefreshToken { get; set; }

    public LoggedResponse()
    {
        Token = string.Empty;
        RefreshToken = string.Empty;
    }

    public LoggedResponse(string token, DateTime expiration, string refreshToken)
    {
        Token = token;
        Expiration = expiration;
        RefreshToken = refreshToken;
    }
}
