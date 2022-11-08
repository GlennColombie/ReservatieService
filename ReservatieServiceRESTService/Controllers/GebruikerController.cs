using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservatieServiceBL.Managers;
using ReservatieServiceBL.Model;

namespace ReservatieServiceRESTService.Controllers
{
    [Route("api/[controller]")]
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
    }
}
