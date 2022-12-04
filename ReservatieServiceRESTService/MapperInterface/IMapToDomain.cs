using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Managers;
using ReservatieServiceGebruikerRESTService.Model.Input;

namespace ReservatieServiceGebruikerRESTService.MapperInterface
{
    public interface IMapToDomain
    {
        Locatie MapToLocatieDomain(LocatieRESTinputDTO dto);

        Gebruiker MapToGebruikerDomain(GebruikerRESTinputDTO dto, LocatieManager lm);

        Gebruiker MapToGebruikerDomain(GebruikerRESTinputUpdateDTO dto, LocatieManager lm, GebruikerManager gm);

        Reservatie MapToReservatieDomain(int gebruikerId, int restaurantId, ReservatieRESTinputDTO dto, GebruikerManager gm, RestaurantManager rm, LocatieManager lm);

        Reservatie MapToReservatieDomain(int reservatienummer, ReservatieRESTinputUpdateDTO dto, ReservatieManager rm);
    }
}
