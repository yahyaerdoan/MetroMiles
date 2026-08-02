using Core.ApplicationLayer.Requests.Page;
using MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Transmissions.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Transmissions.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Transmissions.Queries.GetList;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;

using ResultHandler.AspNetCore.Extensions;
using ResultHandler.Core.Base;

namespace MetroMiles.WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransmissionsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateTransmissionCommand createTransmissionCommand)
    {
        var result = await Mediator.Send(createTransmissionCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListTransmissionQuery getListTransmissionQuery = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListTransmissionQuery, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdTransmissionQuery getByIdTransmissionQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdTransmissionQuery, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateTransmissionCommand updateTransmissionCommand)
    {
        var result = await Mediator.Send(updateTransmissionCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteTransmissionCommand deleteTransmissionCommand)
    {
        var result = await Mediator.Send(deleteTransmissionCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }
}
