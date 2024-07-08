using Core.ApplicationLayer.Requests.Page;
using Core.ApplicationLayer.Responses.GetList;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetList;
using MetroMiles.WebApiLayer.Controllers.BaseControllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetroMiles.WebApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModelsController : BaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest) 
        { 
            GetListModelQuery getListModelQuery = new () { PageRequest = pageRequest };
            GetListResponse<GetListModelListItemDto> response = await Mediator.Send(getListModelQuery);
            return Ok(response);
        }
    }
}
