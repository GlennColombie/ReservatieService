using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Managers;
using ReservatieServiceGebruikerRESTService.Exceptions;
using ReservatieServiceGebruikerRESTService.Model.Output;

namespace ReservatieServiceGebruikerRESTService.Mappers
{
    public static class MapFromDomain
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

        public static GebruikerRESToutputDTO MapFromGebruikerDomain(Gebruiker gebruiker)
        {
            try
            {
                List<ReservatieRESToutputDTO> reservatiedto = new();
                foreach (Reservatie reservatie in gebruiker.Reservaties) reservatiedto.Add(MapFromReservatieDomain(reservatie));
                LocatieRESToutputDTO ldto = MapFromLocatieDomain(gebruiker.Locatie);
                if (reservatiedto.Count == 0) return new GebruikerRESToutputDTO(gebruiker.GebruikerId, gebruiker.Naam, gebruiker.Email, gebruiker.Telefoonnummer, ldto, null);
                else return new GebruikerRESToutputDTO(gebruiker.GebruikerId, gebruiker.Naam, gebruiker.Email, gebruiker.Telefoonnummer, ldto, reservatiedto);
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromGebruikerDomain", ex);
            }
        }

        public static RestaurantRESToutputDTO MapFromRestaurantDomain(Restaurant restaurant)
        {
            try
            {
                List<TafelRESToutputDTO> tafeldto = new();
                foreach (Tafel t in restaurant.Tafels) tafeldto.Add(MapFromTafelDomain(t));
                LocatieRESToutputDTO ldto = MapFromLocatieDomain(restaurant.Locatie);
                return new RestaurantRESToutputDTO(restaurant.Id, restaurant.Naam, restaurant.Email, restaurant.Telefoonnummer, ldto, tafeldto);
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromRestaurantDomain", ex);
            }
        }

        public static ReservatieRESToutputDTO MapFromReservatieDomain(Reservatie reservatie)
        {
            try
            {
                RestaurantRESToutputDTO rdto = MapFromRestaurantDomain(reservatie.Restaurant);
                TafelRESToutputDTO tdto = MapFromTafelDomain(reservatie.Tafel);
                return new ReservatieRESToutputDTO(reservatie.Reservatienummer, reservatie.Tafelnummer, reservatie.Datum, reservatie.Uur, reservatie.Einduur, reservatie.AantalPlaatsen, rdto, tdto);
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
