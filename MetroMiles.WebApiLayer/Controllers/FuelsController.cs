using Core.ApplicationLayer.Requests.Page;
using MetroMiles.ApplicationLayer.Features.Fuels.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Fuels.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Fuels.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Fuels.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetList;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;

using ResultHandler.AspNetCore.Extensions;

namespace MetroMiles.WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FuelsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateFuelCommand createFuelCommand)
    {
        var result = await Mediator.Send(createFuelCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListFuelQuery getListFuelQuery = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListFuelQuery, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdFuelQuery getByIdFuelQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdFuelQuery, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateFuelCommand updateFuelCommand)
    {
        var result = await Mediator.Send(updateFuelCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteFuelCommand deleteFuelCommand)
    {
        var result = await Mediator.Send(deleteFuelCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpPost("Restore")]
    public async Task<IActionResult> Restore([FromQuery] RestoreFuelCommand restoreFuelCommand)
    {
        var result = await Mediator.Send(restoreFuelCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }
}
