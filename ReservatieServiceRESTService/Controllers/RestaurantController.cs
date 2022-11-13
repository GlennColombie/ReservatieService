using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservatieServiceBL.Managers;
using ReservatieServiceBL.Model;

namespace ReservatieServiceGebruikerRESTService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private RestaurantManager _rM;
        public RestaurantController(RestaurantManager rM)
        {
            _rM = rM;
        }

        [HttpGet]
        public ActionResult<List<Restaurant>> GetRestaurants([FromQuery] int? postcode, string? keuken = null)
        {
            try
            {
                return Ok(_rM.GeefRestaurants(postcode, keuken));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
