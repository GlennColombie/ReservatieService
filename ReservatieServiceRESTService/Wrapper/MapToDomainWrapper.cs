using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Managers;
using ReservatieServiceGebruikerRESTService.MapperInterface;
using ReservatieServiceGebruikerRESTService.Mappers;
using ReservatieServiceGebruikerRESTService.Model.Input;

namespace ReservatieServiceGebruikerRESTService.Wrapper
{
    public class MapToDomainWrapper : IMapToDomain
    {
        public Gebruiker MapToGebruikerDomain(GebruikerRESTinputDTO dto, LocatieManager lm)
        {
            return MapToDomain.MapToGebruikerDomain(dto, lm);
        }

        public Gebruiker MapToGebruikerDomain(GebruikerRESTinputUpdateDTO dto, LocatieManager lm, GebruikerManager gm)
        {
            return MapToDomain.MapToGebruikerDomain(dto, lm, gm);
        }

        public Locatie MapToLocatieDomain(LocatieRESTinputDTO dto)
        {
            return MapToDomain.MapToLocatieDomain(dto);
        }

        public Reservatie MapToReservatieDomain(int gebruikerId, int restaurantId, ReservatieRESTinputDTO dto, GebruikerManager gm, RestaurantManager rm, LocatieManager lm)
        {
            return MapToDomain.MapToReservatieDomain(gebruikerId, restaurantId, dto, gm, rm, lm);
        }

        public Reservatie MapToReservatieDomain(int reservatienummer, ReservatieRESTinputUpdateDTO dto, ReservatieManager rm)
        {
            return MapToDomain.MapToReservatieDomain(reservatienummer, dto, rm);
        }
    }
}
