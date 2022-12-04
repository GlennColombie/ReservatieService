using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservatieServiceBL.Managers;
using ReservatieServiceBL.Entities;
using ReservatieServiceGebruikerRESTService.Model.Input;
using ReservatieServiceGebruikerRESTService.Mappers;
using ReservatieServiceGebruikerRESTService.Model.Output;
using ReservatieServiceGebruikerRESTService.MapperInterface;

namespace ReservatieServiceGebruikerRESTService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReservatieController : ControllerBase
    {
        private ReservatieManager _rM;
        private GebruikerManager _gM;
        private RestaurantManager _restM;
        private LocatieManager _lM;
        private IMapToDomain _mapperToDomain;
        private IMapFromDomain _mapperFromDomain;

        public ReservatieController(IMapToDomain mapper, IMapFromDomain mapper2, ReservatieManager rM, GebruikerManager gM, RestaurantManager restM, LocatieManager lM)
        {
            _mapperToDomain = mapper;
            _mapperFromDomain = mapper2;
            _rM = rM;
            _gM = gM;
            _restM = restM;
            _lM = lM;
        }


        [HttpPost]
        public ActionResult<ReservatieRESToutputDTO> Post(int gebruikerId, int restaurantId, [FromBody] ReservatieRESTinputDTO dto)
        {
            if (gebruikerId <= 0) return BadRequest("GebruikerId moet groter zijn dan 0");
            if (restaurantId <= 0) return BadRequest("RestaurantId moet groter zijn dan 0");
            try
            {
                Reservatie r = _mapperToDomain.MapToReservatieDomain(gebruikerId, restaurantId, dto, _gM, _restM, _lM);
                _rM.VoegReservatieToe(r);
                return Ok(_mapperFromDomain.MapFromReservatieDomain(r));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{reservatieId}")]
        public ActionResult Delete(int reservatieId)
        {
            if (reservatieId <= 0) return BadRequest("ReservatieId moet groter zijn dan 0");
            try
            {
                _rM.AnnuleerReservatie(reservatieId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{reservatieId}")]
        public IActionResult Put(int reservatieId, ReservatieRESTinputUpdateDTO dto)
        {
            if (reservatieId <= 0) return BadRequest("ReservatieId moet groter zijn dan 0");
            try
            {
                Reservatie r = _mapperToDomain.MapToReservatieDomain(reservatieId, dto, _rM);
                _rM.UpdateReservatie(r);
                return Ok(_mapperFromDomain.MapFromReservatieDomain(r));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{gebruikerId}")]
        public ActionResult<List<Reservatie>> GetReservaties(int gebruikerId, string? startdatum, string? einddatum)
        {
            if (gebruikerId <= 0) return BadRequest("GebruikerId moet groter zijn dan 0");
            try
            {
                if (_gM.GeefGebruiker(gebruikerId) == null) return BadRequest("Gebruiker bestaat niet");
                Gebruiker g = _gM.GeefGebruiker(gebruikerId);
                var gdto = _mapperFromDomain.MapFromGebruikerDomain(g);
                List<ReservatieRESToutputDTO> reservaties = new();
                var list = _gM.ZoekReservaties(g, startdatum, einddatum);
                foreach(var r in _gM.ZoekReservaties(g, startdatum, einddatum))
                {
                    reservaties.Add(_mapperFromDomain.MapFromReservatieDomain(r));
                }
                return Ok(reservaties);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
