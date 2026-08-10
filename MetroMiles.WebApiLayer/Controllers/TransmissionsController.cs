using Core.ApplicationLayer.Requests.Page;
using Hateoas;
using Hateoas.AspNetCore;
using MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Transmissions.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Transmissions.Queries.GetList;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResultHandler.AspNetCore.Extensions;

namespace MetroMiles.WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransmissionsController : BaseController
{
    internal const string GetListRouteName = "GetTransmissionsList";
    private const string GetByIdRouteName = "GetTransmissionById";
    private const string UpdateRouteName = "UpdateTransmission";
    private const string DeleteRouteName = "DeleteTransmission";
    private const string RestoreRouteName = "RestoreTransmission";

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateTransmissionCommand createTransmissionCommand)
    {
        var result = await Mediator.Send(createTransmissionCommand, HttpContext.RequestAborted);
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
        GetListTransmissionQuery getListTransmissionQuery = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListTransmissionQuery, HttpContext.RequestAborted);
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
        GetByIdTransmissionQuery getByIdTransmissionQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdTransmissionQuery, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: result.Data.DeletedDate is not null);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpPut("{id}", Name = UpdateRouteName)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTransmissionCommand updateTransmissionCommand)
    {
        updateTransmissionCommand.Id = id;
        var result = await Mediator.Send(updateTransmissionCommand, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: false);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpDelete("{id}", Name = DeleteRouteName)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new DeleteTransmissionCommand { Id = id }, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: true);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpPost("{id}/Restore", Name = RestoreRouteName)]
    public async Task<IActionResult> Restore([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new RestoreTransmissionCommand { Id = id }, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: false);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    private Dictionary<string, Link> BuildLinks(Guid id, bool isDeleted) =>
        this.Links().AddSoftDeletableResourceLinks(id, isDeleted, GetByIdRouteName, UpdateRouteName, DeleteRouteName, RestoreRouteName).Build();
}
