using Core.ApplicationLayer.Requests.Page;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Users.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Users.Queries.GetList;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;

using ResultHandler.AspNetCore.Extensions;
using ResultHandler.Core.Base;

namespace MetroMiles.WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand)
    {
        var result = await Mediator.Send(createUserCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListUserQuery getListUserQuery = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListUserQuery, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        GetByIdUserQuery getByIdUserQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdUserQuery, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand updateUserCommand)
    {
        var result = await Mediator.Send(updateUserCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteUserCommand deleteUserCommand)
    {
        var result = await Mediator.Send(deleteUserCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpPost("Restore")]
    public async Task<IActionResult> Restore([FromQuery] RestoreUserCommand restoreUserCommand)
    {
        var result = await Mediator.Send(restoreUserCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }
}
