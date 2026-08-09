using Core.ApplicationLayer.Requests.Page;
using MetroMiles.ApplicationLayer.Features.Fuels.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Fuels.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Fuels.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Fuels.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetList;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResultHandler.AspNetCore.Extensions;

namespace MetroMiles.WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FuelsController : BaseController
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateFuelCommand createFuelCommand)
    {
        var result = await Mediator.Send(createFuelCommand, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListFuelQuery getListFuelQuery = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListFuelQuery, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdFuelQuery getByIdFuelQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdFuelQuery, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateFuelCommand updateFuelCommand)
    {
        var result = await Mediator.Send(updateFuelCommand, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteFuelCommand deleteFuelCommand)
    {
        var result = await Mediator.Send(deleteFuelCommand, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpPost("{id}/Restore")]
    public async Task<IActionResult> Restore([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new RestoreFuelCommand { Id = id }, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }
}
