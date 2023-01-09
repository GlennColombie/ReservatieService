using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservatieServiceBL.Managers;
using ReservatieServiceBL.Entities;
using ReservatieServiceGebruikerRESTService.Model.Output;
using ReservatieServiceGebruikerRESTService.Mappers;
using ReservatieServiceGebruikerRESTService.Model.Input;
using ReservatieServiceGebruikerRESTService.MapperInterface;

namespace ReservatieServiceRESTService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GebruikerController : ControllerBase
    {
        private GebruikerManager _gM;
        private LocatieManager _lM;
        private IMapToDomain _mapperToDomain;
        private IMapFromDomain _mapperFromDomain;

        public GebruikerController(IMapToDomain mapper, IMapFromDomain mapper2, GebruikerManager gM, LocatieManager lM)
        {
            _mapperToDomain = mapper;
            _mapperFromDomain = mapper2;
            _gM = gM;
            _lM = lM;
        }

        [HttpGet("{gebruikerId}")]
        public ActionResult<GebruikerRESToutputDTO> Get(int gebruikerId)
        {
            if (gebruikerId <= 0) return BadRequest("GebruikerId moet groter zijn dan 0");
            try
            {
                var g = _gM.GeefGebruiker(gebruikerId);
                var dto = _mapperFromDomain.MapFromGebruikerDomain(g);
                return Ok(dto);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost]
        public ActionResult<GebruikerRESToutputDTO> Post([FromBody] GebruikerRESTinputDTO gebruiker)
        {
            if (gebruiker == null) return BadRequest("Gebruiker is null");
            try
            {
                var g = _mapperToDomain.MapToGebruikerDomain(gebruiker, _lM);
                _gM.GebruikerRegistreren(g);
                return Ok(gebruiker);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] GebruikerRESTinputUpdateDTO gebruiker)
        {
            if (id <= 0) return BadRequest("GebruikerId moet groter zijn dan 0");
            if (gebruiker == null) return BadRequest("Gebruiker is null");
            try
            {
                if (id != gebruiker.GebruikerId) return BadRequest("Invalid id!");
                if (!_gM.BestaatGebruiker(id)) return NotFound("Gebruiker niet gevonden");
                var g = _mapperToDomain.MapToGebruikerDomain(gebruiker, _lM, _gM);
                _gM.GebruikerUpdaten(g);
                return CreatedAtAction(nameof(Get), new { gebruikerId = gebruiker.GebruikerId }, _mapperFromDomain.MapFromGebruikerDomain(g));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("GebruikerId moet groter zijn dan 0");
            try
            {
                var g = _gM.GeefGebruiker(id);
                _gM.GebruikerVerwijderen(g);
                return NoContent();
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
