using ReservatieServiceBeheerderRESTService.Exceptions;
using ReservatieServiceBeheerderRESTService.Model.Input;
using ReservatieServiceBL.Entities;
using ReservatieServiceBL.Managers;

namespace ReservatieServiceBeheerderRESTService.Mappers
{
    public static class MapToDomain
    {
        public static Restaurant MapToRestaurantDomain(RestaurantRESTinputDTO dto, LocatieManager lm)
        {
            try
            {
                Locatie l = lm.GeefLocatie(MapToLocatieDomain(dto.Locatie));
                Restaurant r = new(dto.Naam, dto.Email, dto.Telefoonnummer, dto.Keuken)
                {
                    Locatie = l,
                    LocatieId = l.LocatieId
                };
                return r;
            }
            catch (Exception ex)
            {
                throw new MapException("MapToRestaurantDomain", ex);
            }
        }
        
        public static Restaurant MapToRestaurantDomain(int id, RestaurantRESTinputUpdateDTO dto, LocatieManager lm, RestaurantManager rm)
        {
            Locatie l = lm.GeefLocatie(MapToLocatieDomain(dto.Locatie));
            Restaurant r = rm.GeefRestaurant(id);
            r.ZetNaam(dto.Naam);
            r.ZetEmail(dto.Email);
            r.ZetTelefoonnummer(dto.Telefoonnummer);
            r.ZetLocatie(l);
            r.ZetLocatieId();
            return r;
        }

        public static Tafel MapToTafelDomain(TafelRESTinputDTO dto)
        {
            try
            {
                return new Tafel(dto.Tafelnummer, dto.AantalPlaatsen);
            }
            catch (Exception ex)
            {
                throw new MapException("MapToTafelDomain", ex);
            }
        }

        public static Locatie MapToLocatieDomain(LocatieRESTinputDTO dto)
        {
            try
            {
                return new Locatie(dto.Postcode, dto.Gemeente, dto.Straat, dto.Huisnummer);
            }
            catch (Exception ex)
            {
                throw new MapException("MapToLocatieDomain", ex);
            }
        }
    }
}
