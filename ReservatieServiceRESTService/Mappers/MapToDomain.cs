using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Managers;
using ReservatieServiceGebruikerRESTService.Exceptions;
using ReservatieServiceGebruikerRESTService.Model.Input;

namespace ReservatieServiceGebruikerRESTService.Mappers
{
    public static class MapToDomain
    {
        public static Locatie MapToLocatieDomain(LocatieRESTinputDTO dto)
        {
            try
            {
                return new(dto.Postcode, dto.Gemeente, dto.Straat, dto.Huisnummer);
            }
            catch (Exception ex)
            {
                throw new MapException("MapToLocatieDomain", ex);
            }
        }

        public static Gebruiker MapToGebruikerDomain(GebruikerRESTinputDTO dto, LocatieManager lm)
        {
            try
            {
                Locatie l = lm.GeefLocatie(MapToLocatieDomain(dto.Locatie));
                if (l == null) lm.VoegLocatieToe(MapToLocatieDomain(dto.Locatie));
                Gebruiker g = new(dto.Naam, dto.Email, dto.Telefoonnummer);
                g.ZetLocatie(l);
                return g;
            }
            catch (Exception ex)
            {
                throw new MapException("MapToGebruikerDomain", ex);
            }
        }

        public static Gebruiker MapToGebruikerDomain(GebruikerRESTinputUpdateDTO dto, LocatieManager lm, GebruikerManager gm)
        {
            try
            {
                Locatie l = MapToLocatieDomain(dto.Locatie);
                Locatie dbl = lm.GeefLocatie(l);
                var db = gm.GeefGebruiker(dto.GebruikerId);
                db.ZetNaam(dto.Naam);
                db.ZetEmail(dto.Email);
                db.ZetTelefoonnummer(dto.Telefoonnummer);
                db.ZetLocatie(dbl);
                return db;
            }
            catch (Exception ex)
            {
                throw new MapException("MapToGebruikerDomain", ex);
            }
        }

        public static Reservatie MapToReservatieDomain(int gebruikerId, int restaurantId, ReservatieRESTinputDTO dto, GebruikerManager gm, RestaurantManager rm, LocatieManager lm)
        {
            try
            {
                Gebruiker g = gm.GeefGebruiker(gebruikerId);
                Restaurant r = rm.GeefRestaurant(restaurantId);
                Tafel t = rm.GeefTafel(dto.Tafel.Tafelnummer, r);
                Reservatie res = new(dto.Datum, dto.Uur, dto.AantalPlaatsen);
                res.Restaurant = r;
                res.Tafel = t;
                res.Gebruiker = g;
                res.Tafelnummer = t.Tafelnummer;
                res.GebruikerId = g.GebruikerId;
                res.RestaurantId = r.Id;
                return res;
            }
            catch (Exception ex)
            {
                throw new MapException("MapToReservatieDomain", ex);
            }
        }

        public static Reservatie MapToReservatieDomain(int reservatienummer, ReservatieRESTinputUpdateDTO dto, ReservatieManager rm)
        {
            Reservatie r = rm.GeefReservatie(reservatienummer);
            r.ZetDatum(dto.Datum);
            r.ZetUur(dto.Uur);
            r.ZetEinduur();
            r.ZetAantalPlaatsen(dto.AantalPlaatsen);
            return r;
        }
    }
}
