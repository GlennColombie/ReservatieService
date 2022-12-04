using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Entities;
using System.Globalization;
using System.Linq.Expressions;

namespace ReservatieServiceBL.Managers;

public class RestaurantManager
{
    private IRestaurantRepository _restaurantRepository;
    private ILocatieRepository _locatieRepository;
    public RestaurantManager(IRestaurantRepository restaurantRepository, ILocatieRepository locatieRepository)
    {
        _restaurantRepository = restaurantRepository;
        _locatieRepository = locatieRepository;
    }

    public RestaurantManager()
    {
    }

    #region Restaurant

    public virtual void VoegRestaurantToe(Restaurant restaurant)
    {
        if (restaurant == null) throw new RestaurantException("VoegRestaurantToe - null");
        try
        {
            if (!_locatieRepository.BestaatLocatie(restaurant.Locatie)) _locatieRepository.VoegLocatieToe(restaurant.Locatie);
            Locatie l = _locatieRepository.GeefLocatie(restaurant.Locatie);
            if (_restaurantRepository.BestaatRestaurant(restaurant)) throw new RestaurantException("VoegRestaurantToe - bestaat al");
            restaurant.ZetLocatieId();
            _restaurantRepository.VoegRestaurantToe(restaurant);
        }
        catch (Exception ex)
        {
            throw new RestaurantManagerException("VoegRestaurantToe", ex);
        }
    }

    public virtual void UpdateRestaurant(Restaurant restaurant)
    {
        if (restaurant == null) throw new RestaurantException("UpdateRestaurant - null");
        try
        {

            if (_restaurantRepository.BestaatRestaurant(restaurant.Id))
            {
                Restaurant r = _restaurantRepository.GeefRestaurant(restaurant.Id);
                if (r.IsHetzelfde(restaurant)) throw new RestaurantManagerException("UpdateRestaurant - IsHetzelfde");
                if (!_locatieRepository.BestaatLocatie(restaurant.Locatie)) _locatieRepository.VoegLocatieToe(restaurant.Locatie);
                _restaurantRepository.UpdateRestaurant(restaurant);
            }
            else throw new RestaurantManagerException("UpdateRestaurant - bestaat niet");
        }
        catch (Exception ex)
        {
            throw new RestaurantException("UpdateRestaurant", ex);
        }
    }
    
    public virtual bool BestaatRestaurant(int id)
    {
        try
        {
            return _restaurantRepository.BestaatRestaurant(id);
        }
        catch (Exception ex)
        {
            throw new RestaurantManagerException("BestaatRestaurant", ex);
        }
    }

    public virtual void VerwijderRestaurant(Restaurant restaurant)
    {
        if (restaurant == null) throw new RestaurantException("VerwijderRestaurant - null");
        try
        {
            if (!_restaurantRepository.BestaatRestaurant(restaurant)) throw new RestaurantManagerException("VerwijderRestaurant - bestaat niet");
            _restaurantRepository.VerwijderRestaurant(restaurant);
        }
        catch (Exception ex)
        {
            throw new RestaurantManagerException("VerwijderRestaurant", ex);
        }
    }

    public virtual IReadOnlyList<Restaurant> GeefRestaurants()
    {
        try
        {
            return _restaurantRepository.GeefRestaurants();
        }
        catch (Exception ex)
        {
            throw new RestaurantManagerException("GeefAlleRestaurants", ex);
        }
    }

    public virtual Restaurant GeefRestaurant(int id)
    {
        try
        {
            if (id < 0) throw new RestaurantManagerException("Id < 0");
            if (!_restaurantRepository.BestaatRestaurant(id)) throw new RestaurantManagerException("GeefRestaurant - bestaat niet");
            return _restaurantRepository.GeefRestaurant(id);
        }
        catch (Exception ex)
        {
            throw new RestaurantManagerException("GeefRestaurant", ex);
        }
    }

    public virtual IReadOnlyList<Restaurant> GeefRestaurants(int? postcode, string? keuken)
    {
        try
        {
            if (postcode.HasValue && !string.IsNullOrWhiteSpace(keuken))
            {

                return _restaurantRepository.GeefRestaurants(postcode.Value, Enum.Parse<Keuken>(keuken, true));
            }
            else if (postcode.HasValue)
            {
                return _restaurantRepository.GeefRestaurants(postcode.Value);
            }
            else if (!string.IsNullOrWhiteSpace(keuken))
            {
                return _restaurantRepository.GeefRestaurants(null, Enum.Parse<Keuken>(keuken, true));
            }
            else
            {
                throw new RestaurantManagerException("Lege parameters, GeefRestaurants(postcode, keuken)");
            }
        }
        catch (Exception ex)
        {
            throw new RestaurantManagerException("GeefRestaurants", ex);
        }
    }

    #endregion

    #region Tafels

    public virtual void VoegTafelToe(Tafel tafel, Restaurant restaurant)
    {
        if (tafel == null) throw new RestaurantManagerException("AddTafel: Tafel mag niet null zijn");
        if (restaurant == null) throw new RestaurantManagerException("AddTafel: Restaurant mag niet null zijn");
        try
        {
            if (!_restaurantRepository.BestaatRestaurant(restaurant.Id)) throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant - restaurant bestaat niet");
            if (_restaurantRepository.BestaatTafel(tafel.Tafelnummer, restaurant)) throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant - tafel bestaat al");
            _restaurantRepository.VoegTafelToe(tafel, restaurant);
        }
        catch (Exception ex)
        {
            throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant", ex);
        }
    }

    public virtual void VerwijderTafel(Tafel tafel, Restaurant restaurant)
    {
        if (tafel == null) throw new RestaurantManagerException("DeleteTafel: Tafel mag niet null zijn");
        if (restaurant == null) throw new RestaurantManagerException("DeleteTafel: Restaurant mag niet null zijn");
        try
        {
            if (!_restaurantRepository.BestaatRestaurant(restaurant.Id)) throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant - restaurant bestaat niet");
            if (!_restaurantRepository.BestaatTafel(tafel.Tafelnummer, restaurant)) throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant - tafel bestaat niet");
            _restaurantRepository.VerwijderTafel(tafel, restaurant);
        }
        catch (Exception ex)
        {
            throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant", ex);
        }
    }

    public virtual void UpdateTafel(Tafel tafel, Restaurant restaurant)
    {
        if (tafel == null) throw new RestaurantManagerException("UpdateTafel: Tafel mag niet null zijn");
        if (restaurant == null) throw new RestaurantManagerException("UpdateTafel: Restaurant mag niet null zijn");
        try
        {
            if (!_restaurantRepository.BestaatRestaurant(restaurant.Id)) throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant - restaurant bestaat niet");
            if (!_restaurantRepository.BestaatTafel(tafel.Tafelnummer, restaurant)) throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant - tafel bestaat niet");
            Tafel db = _restaurantRepository.GeefTafel(tafel.Tafelnummer, restaurant);
            if (tafel.IsDezelfde(db)) throw new ReservatieManagerException("Niks gewijzigd");
            _restaurantRepository.UpdateTafel(tafel, restaurant);
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("UpdateTafel", ex);
        }
    }
    
    public virtual Tafel GeefTafel(int tafelnummer, Restaurant restaurant)
    {
        if (tafelnummer <= 0) throw new RestaurantManagerException("GeefTafel: Tafelnummer moet groter zijn dan 0");
        if (restaurant == null) throw new RestaurantManagerException("GeefTafel: Restaurant mag niet null zijn");
        try
        {
            if (!_restaurantRepository.BestaatRestaurant(restaurant.Id)) throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant - restaurant bestaat niet");
            if (!_restaurantRepository.BestaatTafel(tafelnummer, restaurant)) throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant - tafel bestaat niet");
            return _restaurantRepository.GeefTafel(tafelnummer, restaurant);
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("GeefTafel", ex);
        }
    }

    public virtual IReadOnlyList<Tafel> GeefAlleTafelsVanRestaurant(Restaurant restaurant)
    {
        if (restaurant == null) throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant: Restaurant mag niet null zijn");
        try
        {
            if (!_restaurantRepository.BestaatRestaurant(restaurant.Id)) throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant - restaurant bestaat niet");
            return _restaurantRepository.GeefAlleTafelsVanRestaurant(restaurant);
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("GeefAlleTafelsVanRestaurant", ex);
        }
    }

    #endregion

    public virtual IReadOnlyList<Restaurant> GeefRestaurantsMetVrijeTafels(string datum, int aantalPersonen, int? postcode, string? keuken)
    {
        string[] formats = { "dd/MM/yyyy H:mm", "dd-MM-yyyy H:mm", "dd.MM.yyyy H:mm", "dd/MM/yy H:mm", "dd-MM-yy H:mm", "dd.MM.yy H:mm" };
        if (!DateTime.TryParseExact(datum, formats, null, DateTimeStyles.None, out DateTime d)) throw new RestaurantManagerException("GeefRestaurantsMetVrijeTafels - datum niet correct formaat");
        if (d < DateTime.Now) throw new RestaurantManagerException("GeefRestaurantsMetVrijeTafels: Datum mag niet in het verleden liggen");
        if (d.Minute % 30 != 0) throw new ReservatieException("ZetDatum - Datum moet een halfuur of een uur zijn");
        if (aantalPersonen < 1) throw new RestaurantManagerException("GeefRestaurantsMetVrijeTafels: Aantal personen moet minstens 1 zijn");
        try
        {
            if (postcode.HasValue && !string.IsNullOrWhiteSpace(keuken))
            {
                if (postcode < 1000 || postcode > 9999) throw new RestaurantManagerException("GeefRestaurantsMetVrijeTafels - postcode niet correct formaat");
                if (!Enum.TryParse<Keuken>(keuken, out Keuken k)) throw new RestaurantManagerException("GeefRestaurants - keuken bestaat niet");
                return _restaurantRepository.GeefRestaurantsMetVrijeTafels(d, aantalPersonen, postcode.Value, k);
            }
            else if (postcode.HasValue)
            {
                if (postcode < 1000 || postcode > 9999) throw new RestaurantManagerException("GeefRestaurantsMetVrijeTafels - postcode niet correct formaat");
                return  _restaurantRepository.GeefRestaurantsMetVrijeTafels(d, aantalPersonen, postcode.Value);
            }
            else if (!string.IsNullOrWhiteSpace(keuken))
            {
                if (!Enum.TryParse<Keuken>(keuken, out Keuken k)) throw new RestaurantManagerException("GeefRestaurants - keuken bestaat niet");
                return _restaurantRepository.GeefRestaurantsMetVrijeTafels(d, aantalPersonen, k);
            }
            else
            {
                return _restaurantRepository.GeefRestaurantsMetVrijeTafels(d, aantalPersonen);
            }
        }
        catch (Exception ex)
        {
            throw new RestaurantException("GeefRestaurantsMetVrijeTafels", ex);
        }
    }

    public virtual IReadOnlyList<Reservatie> GeefReservatiesRestaurant(Restaurant restaurant, string date, string? eindddatum = null)
    {
        string[] formats = { "dd/MM/yyyy", "dd-MM-yyyy", "dd.MM.yyyy", "dd/MM/yy", "dd-MM-yy", "dd.MM.yy" };
        if (restaurant == null) throw new RestaurantManagerException("GeefReservatiesRestaurant: Restaurant mag niet null zijn");
        try
        {
            if (!_restaurantRepository.BestaatRestaurant(restaurant.Id)) throw new RestaurantManagerException("GeefReservatiesRestaurant - restaurant bestaat niet");
            if (!string.IsNullOrWhiteSpace(eindddatum))
            {
                if (!DateTime.TryParseExact(eindddatum, formats, null, DateTimeStyles.None, out DateTime d)) throw new RestaurantManagerException("GeefRestaurantsMetVrijeTafels - datum niet correct formaat");
                if (!DateTime.TryParseExact(date, formats, null, DateTimeStyles.None, out DateTime d2)) throw new RestaurantManagerException("GeefRestaurantsMetVrijeTafels - datum niet correct formaat");
                return _restaurantRepository.GeefReservatiesRestaurant(restaurant, d2, d);
            }
            else
            {
                if (!DateTime.TryParseExact(date, formats, null, DateTimeStyles.None, out DateTime d)) throw new RestaurantManagerException("GeefRestaurantsMetVrijeTafels - datum niet correct formaat");
                return _restaurantRepository.GeefReservatiesRestaurant(restaurant, d);
            }
        }
        catch (Exception ex)
        {
            throw new RestaurantException("GeefReservatiesRestaurant", ex);
        }
    }
}