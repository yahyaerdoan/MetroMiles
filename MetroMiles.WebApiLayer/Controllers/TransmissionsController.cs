using Core.ApplicationLayer.Requests.Page;
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
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateTransmissionCommand createTransmissionCommand)
    {
        var result = await Mediator.Send(createTransmissionCommand, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListTransmissionQuery getListTransmissionQuery = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListTransmissionQuery, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdTransmissionQuery getByIdTransmissionQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdTransmissionQuery, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateTransmissionCommand updateTransmissionCommand)
    {
        var result = await Mediator.Send(updateTransmissionCommand, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteTransmissionCommand deleteTransmissionCommand)
    {
        var result = await Mediator.Send(deleteTransmissionCommand, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpPost("{id}/Restore")]
    public async Task<IActionResult> Restore([FromRoute] Guid id)
    {
        var result = await Mediator.Send(new RestoreTransmissionCommand { Id = id }, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }
}
