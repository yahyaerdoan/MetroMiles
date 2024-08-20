using MetroMiles.ApplicationLayer.Features.Brands.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Create;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetroMiles.WebApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand)
        {
            CreatedUserResponse response = await Mediator.Send(createUserCommand);
            return Ok(response);
        }
    }
}
