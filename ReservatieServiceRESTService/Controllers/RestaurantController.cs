using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservatieServiceBL.Managers;
using ReservatieServiceBL.Entities;
using ReservatieServiceGebruikerRESTService.Model.Output;
using ReservatieServiceGebruikerRESTService.Mappers;
using ReservatieServiceGebruikerRESTService.MapperInterface;
using System.Globalization;

namespace ReservatieServiceGebruikerRESTService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private RestaurantManager _rM;
        private IMapFromDomain _mapperFromDomain;
        public RestaurantController(IMapFromDomain mapper, RestaurantManager rM)
        {
            _mapperFromDomain = mapper;
            _rM = rM;
        }

        [HttpGet]
        [Route("Restaurants")]
        public ActionResult<List<RestaurantRESToutputDTO>> GetRestaurants([FromQuery] int? postcode, string? keuken = null)
        {
            try
            {
                var restaurants = _rM.GeefRestaurants(postcode, keuken);
                List<RestaurantRESToutputDTO> dto = new();
                foreach (var restaurant in restaurants)
                {
                    dto.Add(_mapperFromDomain.MapFromRestaurantDomain(restaurant));
                }
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Route("VrijeTafels")]
        public ActionResult<List<RestaurantRESToutputDTO>> GetRestaurantsMetVrijeTafels(string datum, int aantalPersonen, [FromQuery] int? postcode, [FromQuery] string? keuken)
        {
            if (aantalPersonen < 1) return BadRequest("Aantal personen moet groter zijn dan 0");
            DateTime d = DateTime.ParseExact(datum, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
            if (d < DateTime.Now) return BadRequest("Datum moet in de toekomst liggen");
            try
            {
                var restaurants = _rM.GeefRestaurantsMetVrijeTafels(datum, aantalPersonen, postcode, keuken);
                List<RestaurantRESToutputDTO> dto = new();
                foreach (var restaurant in restaurants)
                {
                    dto.Add(_mapperFromDomain.MapFromRestaurantDomain(restaurant));
                }
                return Ok(dto);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
