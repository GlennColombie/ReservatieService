using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;

namespace ReservatieServiceBL.Managers;

public class RestaurantManager
{
    private IRestaurantRepository _restaurantRepository;
    private ILocatieRepository _locatieRepository;
    private ITafelRepository _tafelRepository;
    public RestaurantManager(IRestaurantRepository restaurantRepository, ILocatieRepository locatieRepository, ITafelRepository tafelRepository)
    {
        _restaurantRepository = restaurantRepository;
        _locatieRepository = locatieRepository;
        _tafelRepository = tafelRepository;
    }

    public void VoegRestaurantToe(Restaurant restaurant)
    {
        if (restaurant == null) throw new RestaurantException("VoegRestaurantToe - null");
        try
        {
            if (_restaurantRepository.BestaatRestaurant(restaurant)) throw new RestaurantException("VoegRestaurantToe - bestaat al");
            if (!_locatieRepository.BestaatLocatie(restaurant.Locatie)) _locatieRepository.VoegLocatieToe(restaurant.Locatie);
            restaurant.ZetLocatie(_locatieRepository.GeefLocatie(restaurant.Locatie));
            _restaurantRepository.VoegRestaurantToe(restaurant);
        }
        catch (Exception ex)
        {
            throw new RestaurantException("VoegRestaurantToe", ex);
        }
    }

    public void UpdateRestaurant(Restaurant restaurant)
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

    public void VerwijderRestaurant(Restaurant restaurant)
    {
        if (restaurant == null) throw new RestaurantException("VerwijderRestaurant - null");
        try
        {
            if (!_restaurantRepository.BestaatRestaurant(restaurant)) throw new RestaurantManagerException("VerwijderRestaurant - bestaat niet");
            _restaurantRepository.VerwijderRestaurant(restaurant);
        }
        catch (Exception ex)
        {
            throw new RestaurantException("VerwijderRestaurant", ex);
        }
    }

    public IReadOnlyList<Restaurant> GeefAlleRestaurants()
    {
        try
        {
            return _restaurantRepository.GeefAlleRestaurants();
        }
        catch (Exception ex)
        {
            throw new RestaurantException("GeefAlleRestaurants", ex);
        }
    }

    public Restaurant GeefRestaurant(int id)
    {
        try
        {
            if (id < 0) throw new RestaurantManagerException("Id < 0");
            if (!_restaurantRepository.BestaatRestaurant(id)) throw new RestaurantManagerException("GeefRestaurant - bestaat niet");
            return _restaurantRepository.GeefRestaurant(id);
        }
        catch (Exception ex)
        {
            throw new RestaurantException("GeefRestaurant", ex);
        }
    }

    //public IReadOnlyList<Restaurant> GeefRestaurantsVanLocatie(Locatie locatie)
    //{
    //    if (locatie == null) throw new RestaurantException("GeefRestaurantsVanLocatie - null");
    //    try
    //    {
    //        return _restaurantRepository.GeefRestaurantsVanLocatie(locatie);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new RestaurantException("GeefRestaurantsVanLocatie", ex);
    //    }
    //}

    public IReadOnlyList<Restaurant> GeefAlleBestaandeRestaurants()
    {
        try
        {
            return _restaurantRepository.GeefAlleBestaandeRestaurants();
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("GeefAlleBestaandeRestaurants", ex);
        }
    }

    public IReadOnlyList<Tafel> GeefAlleTafelsVanRestaurant(int id)
    {
        try
        {
            if (id < 0) throw new RestaurantManagerException("Id < 0");
            if (!_restaurantRepository.BestaatRestaurant(id)) throw new RestaurantManagerException("GeefAlleTafelsVanRestaurant - bestaat niet");
            return _restaurantRepository.GeefAlleTafelsVanRestaurant(id);
        }
        catch (Exception ex)
        {
            throw new RestaurantException("GeefAlleTafelsVanRestaurant", ex);
        }
    }

    public void VoegTafelToe(Tafel tafel, Restaurant restaurant)
    {
        if (tafel == null) throw new RestaurantManagerException("AddTafel: Tafel mag niet null zijn");
        if (restaurant == null) throw new RestaurantManagerException("AddTafel: Restaurant mag niet null zijn");
        try
        {
            if (_restaurantRepository.GeefAlleTafelsVanRestaurant(restaurant.Id).Contains(tafel)) throw new RestaurantManagerException("AddTafel: Tafel bestaat al");
            _tafelRepository.VoegTafelToe(tafel, restaurant);
            restaurant.VoegTafelToe(tafel);
        }
        catch (Exception ex)
        {
            throw new RestaurantManagerException("AddTafel: " + ex.Message);
        }
    }

    public void VerwijderTafel(Tafel tafel, Restaurant restaurant)
    {
        if (tafel == null) throw new RestaurantManagerException("DeleteTafel: Tafel mag niet null zijn");
        if (restaurant == null) throw new RestaurantManagerException("DeleteTafel: Restaurant mag niet null zijn");
        try
        {
            if (!_restaurantRepository.GeefAlleTafelsVanRestaurant(restaurant.Id).Contains(tafel)) throw new RestaurantManagerException("DeleteTafel: Tafel bestaat niet");
            _tafelRepository.VerwijderTafel(tafel, restaurant);
            restaurant.VerwijderTafel(tafel);
        }
        catch (Exception ex)
        {
            throw new RestaurantManagerException("DeleteTafel: " + ex.Message);
        }
    }

    public void UpdateTafel(Tafel tafel, Restaurant restaurant)
    {
        if (tafel == null) throw new RestaurantManagerException("UpdateTafel: Tafel mag niet null zijn");
        if (restaurant == null) throw new RestaurantManagerException("UpdateTafel: Restaurant mag niet null zijn");
        try
        {
            if (!_restaurantRepository.GeefAlleTafelsVanRestaurant(restaurant.Id).Contains(tafel)) throw new RestaurantManagerException("UpdateTafel: Tafel bestaat niet");
            _tafelRepository.UpdateTafel(tafel, restaurant);
            restaurant.UpdateTafel(tafel);
        }
        catch (Exception ex)
        {
            throw new RestaurantManagerException("UpdateTafel: " + ex.Message);
        }
    }

    public IReadOnlyList<Restaurant> GeefRestaurants(int? postcode, string? keuken)
    {
        try
        {
            if (postcode.HasValue && !string.IsNullOrWhiteSpace(keuken))
            {
                if (!Enum.TryParse<Keuken>(keuken, out Keuken k)) throw new RestaurantManagerException("GeefRestaurants - keuken bestaat niet");
                if (postcode.ToString().Length != 4 || (postcode < 1000 || postcode > 9999)) throw new RestaurantManagerException("GeefRestaurants - postcode niet correct formaat");
                return _restaurantRepository.GeefRestaurants(postcode.Value, k);
            }
            else if (postcode.HasValue)
            {
                if (postcode.ToString().Length != 4 || (postcode < 1000 || postcode > 9999)) throw new RestaurantManagerException("GeefRestaurants - postcode niet correct formaat");
                return _restaurantRepository.GeefRestaurants(postcode.Value);
            }
            else if (!string.IsNullOrWhiteSpace(keuken))
            {
                if (!Enum.TryParse<Keuken>(keuken, out Keuken k)) throw new RestaurantManagerException("GeefRestaurants - keuken bestaat niet");
                return _restaurantRepository.GeefRestaurants(keuken: k);
            }
            else
            {
                return _restaurantRepository.GeefAlleRestaurants();
            }
        }
        catch (Exception ex)
        {
            throw new RestaurantException("GeefRestaurants", ex);
        }
    }

    public Tafel GeefTafel(int id, Restaurant restaurant)
    {
        {
            try
            {
                if (id < 0) throw new RestaurantManagerException("Id < 0");
                if (!_tafelRepository.BestaatTafel(id, restaurant)) throw new RestaurantManagerException("GeefTafel - bestaat niet");
                return _tafelRepository.GeefTafel(id, restaurant);
            }
            catch (Exception ex)
            {
                throw new RestaurantException("GeefTafel", ex);
            }
        }
    }

    //public IReadOnlyList<Tafel> GeefAlleVrijTafelsVanRestaurant(Restaurant restaurant, DateTime datum, TimeSpan beginTijd, TimeSpan eindTijd)
    //{
    //    if (restaurant == null) throw new RestaurantManagerException("GeefAlleVrijTafelsVanRestaurant: Restaurant mag niet null zijn");
    //    try
    //    {
    //        return _tafelRepository.GeefAlleVrijTafelsVanRestaurant(restaurant, datum, beginTijd, eindTijd);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new RestaurantManagerException("GeefAlleVrijTafelsVanRestaurant: " + ex.Message);
    //    }
    //}

    ////public Tafel GeefTafel(int tafelnummer)
    ////{
    ////    if (tafelnummer < 0) throw new TafelManagerException("GetTafel: Tafelnummer moet groter zijn dan 0");
    ////    try
    ////    {
    ////        return _tafelRepository.GeefTafel(tafelnummer);
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        throw new TafelManagerException("DeleteTafel: Tafel mag niet null zijn");
    ////    }
    ////}

    //public List<Restaurant> GeefRestaurantsVanLocatie(int locatieId)
    //{
    //    try
    //    {
    //        return _restaurantRepository.GeefRestaurantsVanLocatie(locatieId);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new RestaurantException("GeefRestaurantsVanLocatie", ex);
    //    }
    //}

    //public List<Restaurant> GeefRestaurantsVanLocatie(string locatieNaam)
    //{
    //    try
    //    {
    //        return _restaurantRepository.GeefRestaurantsVanLocatie(locatieNaam);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new RestaurantException("GeefRestaurantsVanLocatie", ex);
    //    }
    //}
}