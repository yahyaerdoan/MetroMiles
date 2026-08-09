using MetroMiles.ApplicationLayer.Features.Roles.Commands.AddClaim;
using MetroMiles.ApplicationLayer.Features.Roles.Commands.AssignUser;
using MetroMiles.ApplicationLayer.Features.Roles.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Roles.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Roles.Commands.RemoveClaim;
using MetroMiles.ApplicationLayer.Features.Roles.Commands.RemoveUser;
using MetroMiles.ApplicationLayer.Features.Roles.Queries.GetList;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResultHandler.AspNetCore.Extensions;

namespace MetroMiles.WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var result = await Mediator.Send(new GetListRoleQuery(), HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand createRoleCommand)
    {
        var result = await Mediator.Send(createRoleCommand, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new DeleteRoleCommand { Id = id }, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpPost("{id}/claims")]
    public async Task<IActionResult> AddClaim([FromRoute] Guid id, [FromBody] AddRoleClaimRequest request)
    {
        var result = await Mediator.Send(new AddRoleClaimCommand { RoleId = id, ClaimValue = request.ClaimValue }, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpDelete("{id}/claims")]
    public async Task<IActionResult> RemoveClaim([FromRoute] Guid id, [FromBody] AddRoleClaimRequest request)
    {
        var result = await Mediator.Send(new RemoveRoleClaimCommand { RoleId = id, ClaimValue = request.ClaimValue }, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpPost("{roleName}/users/{userId}")]
    public async Task<IActionResult> AssignUser([FromRoute] string roleName, [FromRoute] Guid userId)
    {
        var result = await Mediator.Send(new AssignUserToRoleCommand { UserId = userId, RoleName = roleName }, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpDelete("{roleName}/users/{userId}")]
    public async Task<IActionResult> RemoveUser([FromRoute] string roleName, [FromRoute] Guid userId)
    {
        var result = await Mediator.Send(new RemoveUserFromRoleCommand { UserId = userId, RoleName = roleName }, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    public record AddRoleClaimRequest(string ClaimValue);
}
