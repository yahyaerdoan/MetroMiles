using Core.ApplicationLayer.Requests.Page;
using Core.PersistenceLayer.Dynamics.Dynamic;
using MetroMiles.ApplicationLayer.Features.Models.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Models.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Models.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Models.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetList;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetListByDynamicQuery;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;

using ResultHandler.AspNetCore.Extensions;

namespace MetroMiles.WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModelsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateModelCommand createModelCommand)
    {
        var result = await Mediator.Send(createModelCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListModelQuery getListModelQuery = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListModelQuery, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdModelQuery getByIdModelQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdModelQuery, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateModelCommand updateModelCommand)
    {
        var result = await Mediator.Send(updateModelCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteModelCommand deleteModelCommand)
    {
        var result = await Mediator.Send(deleteModelCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpPost("Restore")]
    public async Task<IActionResult> Restore([FromQuery] RestoreModelCommand restoreModelCommand)
    {
        var result = await Mediator.Send(restoreModelCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }

    [HttpPost("GetList/ByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery? dynamicQuery = null)
    {
        GetListByDynamicModelQuery getListByDynamicModelQuery = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery };
        var result = await Mediator.Send(getListByDynamicModelQuery, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }
}
