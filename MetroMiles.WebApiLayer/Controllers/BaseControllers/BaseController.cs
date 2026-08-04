using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MetroMiles.WebApiLayer.Controllers.BaseControllers;

public class BaseController : ControllerBase
{
    protected IMediator Mediator => field ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
}
