using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservatieServiceBL.Managers;
using ReservatieServiceBL.Model;

namespace ReservatieServiceBeheerderRESTService.Controllers
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
        public ActionResult<List<Restaurant>> GetAll()
        {
            try
            {
                return Ok(_rM.GeefAlleRestaurants());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Restaurant> Get(int id)
        {
            try
            {
                return Ok(_rM.GeefRestaurant(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult<Restaurant> Post([FromBody] Restaurant restaurant)
        {
            if (restaurant == null) return BadRequest("Restaurant is null");
            try
            {
                _rM.VoegRestaurantToe(restaurant);
                return Ok(restaurant);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Restaurant restaurant)
        {
            if (restaurant == null) return BadRequest("Restaurant is null");
            if (restaurant.Id != id) return BadRequest("Restaurant id is niet hetzelfde");
            try
            {
                _rM.UpdateRestaurant(restaurant);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _rM.VerwijderRestaurant(_rM.GeefRestaurant(id));
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("{id}/Tafels")]
        public ActionResult<Tafel> Post(int id, [FromBody] Tafel tafel)
        {
            if (id <= 0) return BadRequest("Id is niet geldig");
            if (tafel == null) return BadRequest("Tafel is null");
            try
            {
                Restaurant restaurant = _rM.GeefRestaurant(id);
                _rM.VoegTafelToe(tafel, restaurant);
                return Ok(tafel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet]
        [Route("{id}/Tafels")]
        public ActionResult<List<Tafel>> GetTafels(int id)
        {
            if (id <= 0) return BadRequest("Id is niet geldig");
            try
            {
                return Ok(_rM.GeefAlleTafelsVanRestaurant(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("{id}/Tafels/{tafelId}")]
        public IActionResult DeleteTafel(int id, int tafelId)
        {
            if (id <= 0) return BadRequest("Id is niet geldig");
            if (tafelId <= 0) return BadRequest("TafelId is niet geldig");
            try
            {
                _rM.VerwijderTafel(_rM.GeefTafel(tafelId));
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
