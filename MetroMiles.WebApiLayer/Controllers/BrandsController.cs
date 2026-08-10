using Core.ApplicationLayer.Requests.Page;
using Hateoas;
using Hateoas.AspNetCore;
using MetroMiles.ApplicationLayer.Features.Brands.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Brands.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Brands.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Brands.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Brands.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Brands.Queries.GetList;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResultHandler.AspNetCore.Extensions;

namespace MetroMiles.WebApiLayer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandsController : BaseController
{
    internal const string GetListRouteName = "GetBrandsList";
    private const string GetByIdRouteName = "GetBrandById";
    private const string UpdateRouteName = "UpdateBrand";
    private const string DeleteRouteName = "DeleteBrand";
    private const string RestoreRouteName = "RestoreBrand";

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateBrandCommand createBrandCommand)
    {
        var result = await Mediator.Send(createBrandCommand, HttpContext.RequestAborted);
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
        GetListBrandQuery getListBrandQuery = new() { PageRequest = pageRequest };
        var result = await Mediator.Send(getListBrandQuery, HttpContext.RequestAborted);
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
    [HttpGet("{id}", Name = GetByIdRouteName)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdBrandQuery getByIdBrandQuery = new() { Id = id };
        var result = await Mediator.Send(getByIdBrandQuery, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: result.Data.DeletedDate is not null);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }
    [Authorize]
    [HttpPut("{id}", Name = UpdateRouteName)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBrandCommand updateBrandCommand)
    {
        updateBrandCommand.Id = id;
        var result = await Mediator.Send(updateBrandCommand, HttpContext.RequestAborted);
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
        var result = await Mediator.Send(new DeleteBrandCommand { Id = id }, HttpContext.RequestAborted);
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
        var result = await Mediator.Send(new RestoreBrandCommand { Id = id }, HttpContext.RequestAborted);
        if (result.IsSuccessful)
        {
            result.Data.Links = BuildLinks(id, isDeleted: false);
        }
        return result.ToEnvelopedActionResult(HttpContext);
    }

    private Dictionary<string, Link> BuildLinks(Guid id, bool isDeleted) =>
        this.Links().AddSoftDeletableResourceLinks(id, isDeleted, GetByIdRouteName, UpdateRouteName, DeleteRouteName, RestoreRouteName).Build();
}
