using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservatieServiceBeheerderRESTService.MapperInterface;
using ReservatieServiceBeheerderRESTService.Mappers;
using ReservatieServiceBeheerderRESTService.Model.Output;
using ReservatieServiceBL.Managers;

namespace ReservatieServiceBeheerderRESTService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReservatieController : ControllerBase
    {
        private RestaurantManager _rM;
        private IMapFromDomain _mapper;

        public ReservatieController(IMapFromDomain mapper, RestaurantManager rM)
        {
            _mapper = mapper;
            _rM = rM;
        }

        [HttpGet("{restaurantId}")]
        public ActionResult<List<ReservatieRESToutputDTO>> Get(int restaurantId, string datum, string? einddatum)
        {
            if (restaurantId <= 0) return BadRequest("RestaurantController - Get(restaurant) - Restaurant id is niet geldig");
            try
            {
                var r = _rM.GeefRestaurant(restaurantId);
                List<ReservatieRESToutputDTO> reservaties = new();
                foreach (var res in _rM.GeefReservatiesRestaurant(r, datum, einddatum)) reservaties.Add(_mapper.MapFromReservatieDomain(res));
                return Ok(reservaties);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
