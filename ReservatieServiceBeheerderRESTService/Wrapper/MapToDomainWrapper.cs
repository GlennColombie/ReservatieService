using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Managers;
using ReservatieServiceBeheerderRESTService.MapperInterface;
using ReservatieServiceBeheerderRESTService.Mappers;
using ReservatieServiceBeheerderRESTService.Model.Input;

namespace ReservatieServiceBeheerderRESTService.Wrapper
{
    public class MapToDomainWrapper : IMapToDomain
    {
        public Locatie MapToLocatieDomain(LocatieRESTinputDTO dto)
        {
            return MapToDomain.MapToLocatieDomain(dto);
        }
        
        public Restaurant MapToRestaurantDomain(RestaurantRESTinputDTO dto, LocatieManager lm)
        {
            return MapToDomain.MapToRestaurantDomain(dto, lm);
        }

        public Restaurant MapToRestaurantDomain(int id, RestaurantRESTinputUpdateDTO dto, LocatieManager lm, RestaurantManager rm)
        {
            return MapToDomain.MapToRestaurantDomain(id, dto, lm, rm);
        }

        public Tafel MapToTafelDomain(TafelRESTinputDTO dto)
        {
            return MapToDomain.MapToTafelDomain(dto);
        }
    }
}
