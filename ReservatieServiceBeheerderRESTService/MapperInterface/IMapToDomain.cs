using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Managers;
using ReservatieServiceBeheerderRESTService.Model.Input;

namespace ReservatieServiceBeheerderRESTService.MapperInterface
{
    public interface IMapToDomain
    {
        Locatie MapToLocatieDomain(LocatieRESTinputDTO dto);
        Restaurant MapToRestaurantDomain(RestaurantRESTinputDTO dto, LocatieManager lm);
        Restaurant MapToRestaurantDomain(int id, RestaurantRESTinputUpdateDTO dto, LocatieManager lm, RestaurantManager rm);
        Tafel MapToTafelDomain(TafelRESTinputDTO dto);
    }
}
