using ReservatieServiceBL.Model;

namespace ReservatieServiceBL.Interfaces;

public interface IRestaurantRepository
{
    bool BestaatRestaurant(Restaurant restaurant);
    bool BestaatRestaurant(int id);
    void VoegRestaurantToe(Restaurant restaurant);
    void UpdateRestaurant(Restaurant restaurant);
    void VerwijderRestaurant(Restaurant restaurant);
    IReadOnlyList<Restaurant> GeefAlleRestaurants();
    IReadOnlyList<Restaurant> GeefAlleBestaandeRestaurants();
    Restaurant GeefRestaurant(int id);
    IReadOnlyList<Restaurant> GeefRestaurants(int? postcode = 0, Keuken? keuken = null);
    //IReadOnlyList<Restaurant> GeefRestaurants(DateTime datum, int aantal);
    IReadOnlyList<Tafel> GeefAlleTafelsVanRestaurant(int id);
}