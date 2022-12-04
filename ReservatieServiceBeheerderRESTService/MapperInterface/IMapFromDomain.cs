using ReservatieServiceBL.Entities;
using ReservatieServiceBeheerderRESTService.Model.Output;

namespace ReservatieServiceBeheerderRESTService.MapperInterface
{
    public interface IMapFromDomain
    {
        LocatieRESToutputDTO MapFromLocatieDomain(Locatie locatie);

        GebruikerRESToutputDTO MapFromGebruikerDomain(Gebruiker gebruiker);

        RestaurantRESToutputDTO MapFromRestaurantDomain(Restaurant restaurant);

        ReservatieRESToutputDTO MapFromReservatieDomain(Reservatie reservatie);

        TafelRESToutputDTO MapFromTafelDomain(Tafel tafel);
    }
}
