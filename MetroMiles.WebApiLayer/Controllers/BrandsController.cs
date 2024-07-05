using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using MetroMiles.ApplicationLayer.Features.Brands.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Brands.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Brands.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Brands.Queries.GetList;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetroMiles.WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandsController : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateBrandCommand createBrandCommand)
    {
        CreatedBrandResponse response = await Mediator.Send(createBrandCommand);
        return Ok(response);
    }
    [HttpGet]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListBrandQuery getListBrandQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListBrandListItemDto> response = await Mediator.Send(getListBrandQuery);
        return Ok(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdBrandQuery getByIdBrandQuery = new() { Id = id };
        GetByIdBrandResponse response = await Mediator.Send(getByIdBrandQuery);
        return Ok(response);
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBrandCommand updateBrandCommand)
    {
        UpdatedBrandResponse response = await Mediator.Send(updateBrandCommand);
        return Ok(response);
    }
}
