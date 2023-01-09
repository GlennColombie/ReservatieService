using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ReservatieServiceBeheerderRESTService.MapperInterface;
using ReservatieServiceBeheerderRESTService.Mappers;
using ReservatieServiceBeheerderRESTService.Model.Input;
using ReservatieServiceBeheerderRESTService.Model.Output;
using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Managers;

namespace ReservatieServiceBeheerderRESTService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private RestaurantManager _rM;
        private LocatieManager _lM;
        private IMapFromDomain _mapperFromDomain;
        private IMapToDomain _mapperToDomain;
        public RestaurantController(IMapToDomain mapper, IMapFromDomain mapper2, RestaurantManager rM, LocatieManager lm)
        {
            _mapperToDomain = mapper;
            _mapperFromDomain = mapper2;
            _rM = rM;
            _lM = lm;
        }

        [HttpGet("{restaurantId}")]
        public ActionResult<RestaurantRESToutputDTO> Get(int restaurantId)
        {
            if (restaurantId <= 0) return BadRequest("RestaurantController - Get(restaurant) - Restaurant id is niet geldig");
            try
            {
                var r = _rM.GeefRestaurant(restaurantId);
                return Ok(_mapperFromDomain.MapFromRestaurantDomain(r));
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public ActionResult<RestaurantRESToutputDTO> Post([FromBody] RestaurantRESTinputDTO restaurant)
        {
            if (restaurant == null) return BadRequest("RestaurantController - Post(restaurant) - Restaurant is null");
            try
            {
                Restaurant r = _mapperToDomain.MapToRestaurantDomain(restaurant, _lM);
                _rM.VoegRestaurantToe(r);
                return Ok(restaurant);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{restaurantId}")]
        public IActionResult Put(int restaurantId, [FromBody] RestaurantRESTinputUpdateDTO restaurant)
        {
            if (restaurantId <= 0) return BadRequest("RestaurantId moet groter zijn dan 0");
            if (restaurant == null) return BadRequest("RestaurantController - Put - Restaurant is null");
            try
            {
                if (restaurant.Id != restaurantId) return BadRequest("RestaurantController - Put - Restaurant id is niet hetzelfde");
                if (_rM.BestaatRestaurant(restaurantId))
                {
                    Restaurant r = _mapperToDomain.MapToRestaurantDomain(restaurantId, restaurant, _lM, _rM);
                    _rM.UpdateRestaurant(r);
                    return CreatedAtAction(nameof(Get), new { restaurantId = r.Id }, _mapperFromDomain.MapFromRestaurantDomain(r));
                }
                else return NotFound();
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
    }
}
