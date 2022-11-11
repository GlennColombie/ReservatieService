using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;

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

    public IReadOnlyList<Restaurant> GeefRestaurantsVanLocatie(Locatie locatie)
    {
        if (locatie == null) throw new RestaurantException("GeefRestaurantsVanLocatie - null");
        try
        {
            return _restaurantRepository.GeefRestaurantsVanLocatie(locatie);
        }
        catch (Exception ex)
        {
            throw new RestaurantException("GeefRestaurantsVanLocatie", ex);
        }
    }

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