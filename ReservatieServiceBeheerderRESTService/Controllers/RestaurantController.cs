using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
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

        [HttpGet("{restaurantId}")]
        public ActionResult<Restaurant> Get(int restaurantId)
        {
            if (restaurantId <= 0) return BadRequest("RestaurantController - Get(restaurant) - Restaurant id is niet geldig");
            try
            {
                return Ok(_rM.GeefRestaurant(restaurantId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult<Restaurant> Post([FromBody] Restaurant restaurant)
        {
            if (restaurant == null) return BadRequest("RestaurantController - Post(restaurant) - Restaurant is null");
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

        [HttpPut("{restaurantId}")]
        public IActionResult Put(int restaurantId, [FromBody] Restaurant restaurant)
        {
            if (restaurant == null) return BadRequest("RestaurantController - Put - Restaurant is null");
            if (restaurant.Id != restaurantId) return BadRequest("RestaurantController - Put - Restaurant id is niet hetzelfde");
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

        [HttpDelete("{restaurantId}")]
        public IActionResult Delete(int restaurantId)
        {
            if (restaurantId <= 0) return BadRequest("RestaurantController - Delete(restaurant) - RestaurantId is niet geldig");
            try
            {
                _rM.VerwijderRestaurant(_rM.GeefRestaurant(restaurantId));
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("{restaurantId}/Tafels")]
        public ActionResult<Tafel> Post(int restaurantId, [FromBody] Tafel tafel)
        {
            if (restaurantId <= 0) return BadRequest("RestaurantController - Post(tafel) - Id is niet geldig");
            if (tafel == null) return BadRequest("RestaurantController - Post(tafel) - Tafel is null");
            try
            {
                Restaurant restaurant = _rM.GeefRestaurant(restaurantId);
                _rM.VoegTafelToe(tafel, restaurant);
                return Ok(tafel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet]
        [Route("{restaurantId}/Tafels")]
        public ActionResult<List<Tafel>> GetTafels(int restaurantId)
        {
            if (restaurantId <= 0) return BadRequest("RestaurantController - Get(tafels) - restaurantId is niet geldig");
            try
            {
                return Ok(_rM.GeefAlleTafelsVanRestaurant(restaurantId));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        [Route("{restaurantId}/Tafels/{tafelnummer}")]
        public IActionResult DeleteTafel(int restaurantId, int tafelnummer)
        {
            if (restaurantId <= 0) return BadRequest("RestaurantController - Delete(tafel) - Id is niet geldig");
            if (tafelnummer <= 0) return BadRequest("RestaurantController - Delete(tafel) - TafelId is niet geldig");
            try
            {
                Restaurant r = _rM.GeefRestaurant(restaurantId);
                Tafel t = _rM.GeefTafel(tafelnummer, r);
                _rM.VerwijderTafel(t, r);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
