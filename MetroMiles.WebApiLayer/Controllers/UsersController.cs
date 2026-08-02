using MetroMiles.ApplicationLayer.Features.Users.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Update;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Mvc;

using ResultHandler.AspNetCore.Extensions;
using ResultHandler.Core.Base;

namespace MetroMiles.WebApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand)
        {
            OperationDataResult<CreatedUserResponse> result = await Mediator.Send(createUserCommand, HttpContext.RequestAborted);
            return result.ToActionResult(HttpContext);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateUserCommand updateUserCommand)
        {
            OperationDataResult<UpdatedUserResponse> result = await Mediator.Send(updateUserCommand, HttpContext.RequestAborted);
            return result.ToActionResult(HttpContext);
        }
    }
}
