using Asp.Versioning;
using Hateoas.AspNetCore;
using MetroMiles.ApplicationLayer.Common;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;
using ResultHandler.AspNetCore.Extensions;
using ResultHandler.Facade;

namespace MetroMiles.WebApiLayer.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}")]
[ApiController]
public class RootController : BaseController
{
    private const string RootRouteName = "ApiRoot";

    // Entry point for discovery: a client should only ever need to know this URL - every other
    // resource is reached by following the links returned here (and further links returned from
    // those resources), never by hardcoding a collection's path.
    [HttpGet(Name = RootRouteName)]
    public IActionResult Get()
    {
        ApiRootResponse response = new()
        {
            Links = this.Links()
                .Add("self", RootRouteName, "GET")
                .Add("brands", BrandsController.GetListRouteName, "GET")
                .Add("cars", CarsController.GetListRouteName, "GET")
                .Add("fuels", FuelsController.GetListRouteName, "GET")
                .Add("models", ModelsController.GetListRouteName, "GET")
                .Add("transmissions", TransmissionsController.GetListRouteName, "GET")
                .Add("users", UsersController.GetListRouteName, "GET")
                .Build(), 
        };
        return Result.Success(response).ToEnvelopedActionResult(HttpContext);
    }
}
