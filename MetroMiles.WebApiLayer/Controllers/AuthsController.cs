using Core.ApplicationLayer.Requests.Page;
using Core.SecurityLayer.Extensions;
using MetroMiles.ApplicationLayer.Features.Auths.Commands.ChangePassword;
using MetroMiles.ApplicationLayer.Features.Auths.Commands.Login;
using MetroMiles.ApplicationLayer.Features.Auths.Commands.RefreshToken;
using MetroMiles.ApplicationLayer.Features.Auths.Commands.RevokeToken;
using MetroMiles.ApplicationLayer.Features.Auths.Commands.RevokeSession;
using MetroMiles.ApplicationLayer.Features.Auths.Queries.GetActiveSessions;
using MetroMiles.ApplicationLayer.Features.Auths.Queries.GetSessionHistory;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ResultHandler.AspNetCore.Extensions;

namespace MetroMiles.WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthsController : BaseController
{
    private const string AuthRateLimiterPolicy = "AuthRateLimiterPolicy";

    [HttpPost("Login")]
    [EnableRateLimiting(AuthRateLimiterPolicy)]
    public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand)
    {
        loginCommand.IpAddress = GetIpAddress();
        var result = await Mediator.Send(loginCommand, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpPost("RefreshToken")]
    [EnableRateLimiting(AuthRateLimiterPolicy)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand refreshTokenCommand)
    {
        refreshTokenCommand.IpAddress = GetIpAddress();
        var result = await Mediator.Send(refreshTokenCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpPost("RevokeToken")]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenCommand revokeTokenCommand)
    {
        revokeTokenCommand.IpAddress = GetIpAddress();
        var result = await Mediator.Send(revokeTokenCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpPost("ChangePassword")]
    [Authorize]
    [EnableRateLimiting(AuthRateLimiterPolicy)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand changePasswordCommand)
    {
        // Always derived from the caller's own token — never from a client-supplied field — so one
        // authenticated user can't change another user's password.
        changePasswordCommand.UserId = HttpContext.User.GetUserId<Guid>();
        var result = await Mediator.Send(changePasswordCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpGet("Sessions")]
    [Authorize]
    public async Task<IActionResult> GetSessionHistory([FromQuery] PageRequest pageRequest)
    {
        GetSessionHistoryQuery query = new() { PageRequest = pageRequest, UserId = HttpContext.User.GetUserId<Guid>() };
        var result = await Mediator.Send(query, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpGet("Sessions/Active")]
    [Authorize]
    public async Task<IActionResult> GetActiveSessions([FromQuery] PageRequest pageRequest)
    {
        GetActiveSessionsQuery query = new() { PageRequest = pageRequest, UserId = HttpContext.User.GetUserId<Guid>() };
        var result = await Mediator.Send(query, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpDelete("Sessions/{sessionId:guid}")]
    [Authorize]
    public async Task<IActionResult> RevokeSession([FromRoute] Guid sessionId)
    {
        RevokeSessionCommand command = new()
        {
            SessionId = sessionId,
            UserId = HttpContext.User.GetUserId<Guid>(),
            IpAddress = GetIpAddress(),
        };
        var result = await Mediator.Send(command, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    private string GetIpAddress() => HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
}
