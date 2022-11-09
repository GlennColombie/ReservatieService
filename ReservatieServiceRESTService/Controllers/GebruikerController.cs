using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservatieServiceBL.Managers;
using ReservatieServiceBL.Model;

namespace ReservatieServiceRESTService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GebruikerController : ControllerBase
    {
        private GebruikerManager _gM;

        public GebruikerController(GebruikerManager gM)
        {
            _gM = gM;
        }

        [HttpGet]
        public ActionResult<List<Gebruiker>> GetAll()
        {
            try
            {
                return Ok(_gM.GeefGebruikers());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public ActionResult<Gebruiker> Post([FromBody] Gebruiker gebruiker)
        {
            if (gebruiker == null) return BadRequest("Gebruiker is null");
            try
            {
                _gM.GebruikerRegistreren(gebruiker);
                return Ok(gebruiker);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Gebruiker gebruiker)
        {
            if (gebruiker == null) return BadRequest("Gebruiker is null");
            if (gebruiker.Id != id) return BadRequest("Gebruiker id is niet hetzelfde");
            try
            {
                _gM.GebruikerUpdaten(gebruiker);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
