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
    IReadOnlyList<Restaurant> GeefRestaurantsVanLocatie(Locatie locatie);
    IReadOnlyList<Tafel> GeefAlleTafelsVanRestaurant(Restaurant restaurant);
}