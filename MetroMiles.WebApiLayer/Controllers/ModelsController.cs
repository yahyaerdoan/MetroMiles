using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Dynamics.Dynamic;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetList;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetListByDynamicQuery;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;

using ResultHandler.AspNetCore.Extensions;
using ResultHandler.Core.Base;

namespace MetroMiles.WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModelsController : BaseController
{
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
    [HttpPost("GetList/ByDynamic")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery? dynamicQuery = null)
    {
        GetListByDynamicModelQuery getListByDynamicModelQuery = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery };
        var result = await Mediator.Send(getListByDynamicModelQuery, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }
}
