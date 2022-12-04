using ReservatieServiceBeheerderRESTService.Exceptions;
using ReservatieServiceBeheerderRESTService.Model.Output;
using ReservatieServiceBL.Entities;

namespace ReservatieServiceBeheerderRESTService.Mappers
{
    public class MapFromDomain
    {
        public static LocatieRESToutputDTO MapFromLocatieDomain(Locatie locatie)
        {
            try
            {
                LocatieRESToutputDTO dto;
                if (string.IsNullOrWhiteSpace(locatie.Straat) && string.IsNullOrWhiteSpace(locatie.Huisnummer)) dto = new(locatie.Postcode, locatie.Gemeente);
                else if (string.IsNullOrWhiteSpace(locatie.Straat)) dto = new(locatie.Postcode, locatie.Gemeente, locatie.Huisnummer);
                else if (string.IsNullOrWhiteSpace(locatie.Huisnummer)) dto = new(locatie.Postcode, locatie.Gemeente, locatie.Huisnummer);
                else dto = new(locatie.Postcode, locatie.Gemeente, locatie.Straat, locatie.Huisnummer);
                return dto;
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromLocatieDomain", ex);
            }
        }

        public static RestaurantRESToutputDTO MapFromRestaurantDomain(Restaurant restaurant)
        {
            try
            {
                List<TafelRESToutputDTO> tafeldto = new();
                foreach (Tafel t in restaurant.Tafels) tafeldto.Add(MapFromTafelDomain(t));
                List<ReservatieRESToutputDTO> rdto = new();
                foreach (Reservatie r in restaurant.Reservaties) rdto.Add(MapFromReservatieDomain(r));
                LocatieRESToutputDTO ldto = MapFromLocatieDomain(restaurant.Locatie);
                return new RestaurantRESToutputDTO(restaurant.Id, restaurant.Naam, restaurant.Email, restaurant.Telefoonnummer, restaurant.Keuken, ldto, tafeldto, rdto);
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromRestaurantDomain", ex);
            }
        }

        public static GebruikerRESToutputDTO MapFromGebruikerDomain(Gebruiker gebruiker)
        {
            try
            {
                LocatieRESToutputDTO ldto = MapFromLocatieDomain(gebruiker.Locatie);
                return new GebruikerRESToutputDTO(gebruiker.GebruikerId, gebruiker.Naam, gebruiker.Email, gebruiker.Telefoonnummer, ldto);
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromGebruikerDomain", ex);
            }
        }

        public static ReservatieRESToutputDTO MapFromReservatieDomain(Reservatie reservatie)
        {
            try
            {
                GebruikerRESToutputDTO g = MapFromGebruikerDomain(reservatie.Gebruiker);
                TafelRESToutputDTO tdto = MapFromTafelDomain(reservatie.Tafel);
                return new ReservatieRESToutputDTO(reservatie.Reservatienummer, reservatie.Tafelnummer, reservatie.Datum, reservatie.Uur, reservatie.Einduur, reservatie.AantalPlaatsen, g, tdto);
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromReservatieDomain", ex);
            }
        }

        public static TafelRESToutputDTO MapFromTafelDomain(Tafel tafel)
        {
            try
            {
                return new TafelRESToutputDTO(tafel.Tafelnummer, tafel.AantalPlaatsen);
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromTafelDomain", ex);
            }
        }
    }
}
