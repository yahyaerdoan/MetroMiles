using Asp.Versioning;
using Core.ApplicationLayer.Requests.Page;
using Hateoas;
using Hateoas.AspNetCore;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Users.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Users.Queries.GetList;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResultHandler.AspNetCore.Extensions;

namespace MetroMiles.WebApiLayer.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[Authorize]
public class UsersController : BaseController
{
    internal const string GetListRouteName = "GetUsersList";
    private const string GetByIdRouteName = "GetUserById";
    private const string UpdateRouteName = "UpdateUser";
    private const string DeleteRouteName = "DeleteUser";
    private const string RestoreRouteName = "RestoreUser";

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand)
    {
        var result = await Mediator.Send(createUserCommand, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(result.Data.Id, isDeleted: false);
            this.SetCreatedLocationHeader(GetByIdRouteName, new { id = result.Data.Id });
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpGet(Name = GetListRouteName)]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListUserQuery getListUserQuery = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListUserQuery, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            foreach (var item in result.Data.Items)
            {
                item.Links = BuildLinks(item.Id, isDeleted: item.DeletedDate is not null);
            }
            this.SetPaginationLinkHeader(GetListRouteName, result.Data.Index, result.Data.Size, result.Data.HasPrevious, result.Data.HasNext, result.Data.Pages);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpGet("{id}", Name = GetByIdRouteName)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdUserQuery getByIdUserQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdUserQuery, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: result.Data.DeletedDate is not null);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpPut("{id}", Name = UpdateRouteName)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserCommand updateUserCommand)
    {
        updateUserCommand.Id = id;
        var result = await Mediator.Send(updateUserCommand, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: false);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpDelete("{id}", Name = DeleteRouteName)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new DeleteUserCommand { Id = id }, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: true);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpPost("{id}/Restore", Name = RestoreRouteName)]
    public async Task<IActionResult> Restore([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new RestoreUserCommand { Id = id }, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: false);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    private Dictionary<string, Link> BuildLinks(Guid id, bool isDeleted) =>
        this.Links().AddSoftDeletableResourceLinks(id, isDeleted, GetByIdRouteName, UpdateRouteName, DeleteRouteName, RestoreRouteName).Build();
}
