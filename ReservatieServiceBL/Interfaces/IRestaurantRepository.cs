using ReservatieServiceBL.Entities;

namespace ReservatieServiceBL.Interfaces;

public interface IRestaurantRepository
{
    bool BestaatRestaurant(Restaurant restaurant);
    bool BestaatRestaurant(int id);
    void VoegRestaurantToe(Restaurant restaurant);
    void UpdateRestaurant(Restaurant restaurant);
    void VerwijderRestaurant(Restaurant restaurant);
    IReadOnlyList<Restaurant> GeefRestaurants();
    Restaurant GeefRestaurant(int id);
    IReadOnlyList<Restaurant> GeefRestaurants(int? postcode = 0, Keuken? keuken = null);
    IReadOnlyList<Restaurant> GeefRestaurantsMetVrijeTafels(DateTime datum, int aantalPersonen);
    IReadOnlyList<Restaurant> GeefRestaurantsMetVrijeTafels(DateTime datum, int aantalPersonen, int postcode);
    IReadOnlyList<Restaurant> GeefRestaurantsMetVrijeTafels(DateTime datum, int aantalPersonen, Keuken keuken);
    IReadOnlyList<Restaurant> GeefRestaurantsMetVrijeTafels(DateTime datum, int aantalPersonen, int postcode, Keuken keuken);
    IReadOnlyList<Tafel> GeefAlleTafelsVanRestaurant(Restaurant restaurant);
    IReadOnlyList<Reservatie> GeefReservatiesRestaurant(Restaurant restaurant, DateTime datum);
    IReadOnlyList<Reservatie> GeefReservatiesRestaurant(Restaurant restaurant, DateTime datum, DateTime einddatum);

    void VoegTafelToe(Tafel tafel, Restaurant restaurant);
    void VerwijderTafel(Tafel tafel, Restaurant restaurant);
    void UpdateTafel(Tafel tafel, Restaurant restaurant);
    Tafel GeefTafel(int tafelnummer, Restaurant restaurant);
    bool BestaatTafel(int tafelnummer, Restaurant restaurant);
}