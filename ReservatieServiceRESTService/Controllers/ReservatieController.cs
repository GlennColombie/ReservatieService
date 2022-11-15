using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservatieServiceBL.Managers;
using ReservatieServiceBL.Model;

namespace ReservatieServiceGebruikerRESTService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservatieController : ControllerBase
    {
        private ReservatieManager _rM;

        public ReservatieController(ReservatieManager rM)
        {
            _rM = rM;
        }

        //[HttpGet]
        //public ActionResult<List<Reservatie>> GetAll()
        //{
        //    try
        //    {
        //        //return Ok(_rM.GeefReservaties());
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        [HttpPost]
        public ActionResult<Reservatie> Post([FromBody] Reservatie reservatie)
        {
            if (reservatie == null) return BadRequest("Reservatie is null");
            try
            {
                _rM.VoegReservatieToe(reservatie);
                return Ok(reservatie);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
