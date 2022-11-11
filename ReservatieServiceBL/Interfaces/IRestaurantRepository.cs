using ReservatieServiceBL.Model;

namespace ReservatieServiceBL.Interfaces;

public interface IRestaurantRepository
{
    bool BestaatRestaurant(Restaurant restaurant);
    void VoegRestaurantToe(Restaurant restaurant);
    void UpdateRestaurant(Restaurant restaurant);
    void VerwijderRestaurant(Restaurant restaurant);
    IReadOnlyList<Restaurant> GeefRestaurants();
}