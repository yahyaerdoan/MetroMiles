using Core.ApplicationLayer.Requests.Page;
using MetroMiles.ApplicationLayer.Features.Brands.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Brands.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Brands.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Brands.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Brands.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Brands.Queries.GetList;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;

using ResultHandler.AspNetCore.Extensions;

namespace MetroMiles.WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateBrandCommand createBrandCommand)
    {
        var result = await Mediator.Send(createBrandCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListBrandQuery getListBrandQuery = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListBrandQuery, HttpContext.RequestAborted);
        return result.ToEnvelopedActionResult(HttpContext);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdBrandQuery getByIdBrandQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdBrandQuery, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBrandCommand updateBrandCommand)
    {
        var result = await Mediator.Send(updateBrandCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete([FromQuery] DeleteBrandCommand deleteBrandCommand)
    {
        var result = await Mediator.Send(deleteBrandCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }
    [HttpPost("Restore")]
    public async Task<IActionResult> Restore([FromQuery] RestoreBrandCommand restoreBrandCommand)
    {
        var result = await Mediator.Send(restoreBrandCommand, HttpContext.RequestAborted);
        return result.ToActionResult(HttpContext);
    }
}
