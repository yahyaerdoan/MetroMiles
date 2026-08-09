using Core.ApplicationLayer.Requests.Page;
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

[Route("api/[controller]")]
[ApiController]
public class CarsController : BaseController
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateCarCommand createCarCommand)
    {
        var result = await Mediator.Send(createCarCommand, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListCarQuery getListCarQuery = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListCarQuery, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdCarQuery getByIdCarQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdCarQuery, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCarCommand updateCarCommand)
    {
        var result = await Mediator.Send(updateCarCommand, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteCarCommand deleteCarCommand)
    {
        var result = await Mediator.Send(deleteCarCommand, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }

    [Authorize]
    [HttpPost("Restore")]
    public async Task<IActionResult> Restore([FromQuery] RestoreCarCommand restoreCarCommand)
    {
        var result = await Mediator.Send(restoreCarCommand, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }
}
