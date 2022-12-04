using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservatieServiceBeheerderRESTService.MapperInterface;
using ReservatieServiceBeheerderRESTService.Mappers;
using ReservatieServiceBeheerderRESTService.Model.Input;
using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Managers;

namespace ReservatieServiceBeheerderRESTService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TafelController : ControllerBase
    {
        private readonly RestaurantManager _rM;
        private readonly LocatieManager _lM;
        private readonly IMapToDomain _mapperToDomain;

        public TafelController(IMapToDomain mapper, RestaurantManager rM, LocatieManager lM)
        {
            _mapperToDomain = mapper;
            _rM = rM;
            _lM = lM;
        }

        [HttpPost]
        public ActionResult Post(int restaurantId, [FromBody] TafelRESTinputDTO tafel)
        {
            if (restaurantId <= 0) return BadRequest("restaurantId <= 0");
            if (tafel == null) return BadRequest("Tafel is null");
            try
            {
                Tafel t = _mapperToDomain.MapToTafelDomain(tafel);
                Restaurant r = _rM.GeefRestaurant(restaurantId);
                _rM.VoegTafelToe(t, r);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(int restaurantId, [FromBody] TafelRESTinputDTO tafel)
        {
            if (restaurantId <= 0) return BadRequest("restaurantId <= 0");
            if (tafel == null) return BadRequest("Tafel is null");
            try
            {
                Tafel t = _mapperToDomain.MapToTafelDomain(tafel);
                Restaurant r = _rM.GeefRestaurant(restaurantId);
                _rM.UpdateTafel(t, r);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{tafelnummer}")]
        public IActionResult Delete(int tafelnummer, int restaurantId)
        {
            if (tafelnummer <= 0) return BadRequest("tafelnummer <= 0");
            if (restaurantId <= 0) return BadRequest("restaurantId <= 0");
            try
            {
                Restaurant r = _rM.GeefRestaurant(restaurantId);
                Tafel t = _rM.GeefTafel(tafelnummer, r);
                _rM.VerwijderTafel(t, r);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
