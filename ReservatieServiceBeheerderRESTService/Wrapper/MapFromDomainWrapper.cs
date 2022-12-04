using ReservatieServiceBL.Entities;
using ReservatieServiceBeheerderRESTService.MapperInterface;
using ReservatieServiceBeheerderRESTService.Mappers;
using ReservatieServiceBeheerderRESTService.Model.Output;

namespace ReservatieServiceBeheerderRESTService.Wrapper
{
    public class MapFromDomainWrapper : IMapFromDomain
    {
        public GebruikerRESToutputDTO MapFromGebruikerDomain(Gebruiker gebruiker)
        {
            return MapFromDomain.MapFromGebruikerDomain(gebruiker);
        }

        public LocatieRESToutputDTO MapFromLocatieDomain(Locatie locatie)
        {
            return MapFromDomain.MapFromLocatieDomain(locatie);
        }

        public ReservatieRESToutputDTO MapFromReservatieDomain(Reservatie reservatie)
        {
            return MapFromDomain.MapFromReservatieDomain(reservatie);
        }

        public RestaurantRESToutputDTO MapFromRestaurantDomain(Restaurant restaurant)
        {
            return MapFromDomain.MapFromRestaurantDomain(restaurant);
        }

        public TafelRESToutputDTO MapFromTafelDomain(Tafel tafel)
        {
            return MapFromDomain.MapFromTafelDomain(tafel);
        }
    }
}
