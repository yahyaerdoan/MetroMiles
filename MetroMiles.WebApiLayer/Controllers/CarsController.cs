using Asp.Versioning;
using Core.ApplicationLayer.Requests.Page;
using Hateoas;
using Hateoas.AspNetCore;
using MetroMiles.ApplicationLayer.Features.Cars.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Cars.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Cars.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Cars.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Cars.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Cars.Queries.GetList;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResultHandler.AspNetCore.Extensions;

namespace MetroMiles.WebApiLayer.Controllers;

[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CarsController : BaseController
{
    internal const string GetListRouteName = "GetCarsList";
    private const string GetByIdRouteName = "GetCarById";
    private const string GetByIdV2RouteName = "GetCarByIdV2";
    private const string UpdateRouteName = "UpdateCar";
    private const string DeleteRouteName = "DeleteCar";
    private const string RestoreRouteName = "RestoreCar";

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCarCommand createCarCommand)
    {
        var result = await Mediator.Send(createCarCommand, HttpContext.RequestAborted);
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
        GetListCarQuery getListCarQuery = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListCarQuery, HttpContext.RequestAborted);
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

    [MapToApiVersion("1.0")]
    [HttpGet("{id}", Name = GetByIdRouteName)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdCarQuery getByIdCarQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdCarQuery, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: result.Data.DeletedDate is not null);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    // v2.0 adds "MileageCategory" to the response - computed and mapped in the application layer,
    // same as every other query, so the controller stays a thin Mediator.Send + Links wrapper.
    [MapToApiVersion("2.0")]
    [HttpGet("{id}", Name = GetByIdV2RouteName)]
    public async Task<IActionResult> GetByIdV2([FromRoute] Guid id)
    {
        GetByIdCarQueryV2 getByIdCarQueryV2 = new() { Id = id };
        var result = await Mediator.Send(getByIdCarQueryV2, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: result.Data.DeletedDate is not null);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpPut("{id}", Name = UpdateRouteName)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCarCommand updateCarCommand)
    {
        updateCarCommand.Id = id;
        var result = await Mediator.Send(updateCarCommand, HttpContext.RequestAborted);
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
        var result = await Mediator.Send(new DeleteCarCommand { Id = id }, HttpContext.RequestAborted);
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
        var result = await Mediator.Send(new RestoreCarCommand { Id = id }, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: false);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    private Dictionary<string, Link> BuildLinks(Guid id, bool isDeleted) =>
        this.Links().AddSoftDeletableResourceLinks(id, isDeleted, GetByIdRouteName, UpdateRouteName, DeleteRouteName, RestoreRouteName).Build();
}
