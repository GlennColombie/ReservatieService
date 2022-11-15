using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;

namespace ReservatieServiceBL.Managers;

public class ReservatieManager
{
    private IReservatieRepository _reservatieRepository;
    private IRestaurantRepository _restaurantRepository;
    private IGebruikerRepository _gebruikerRepository;
    private ITafelRepository _tafelRepository;
    private ILocatieRepository _locatieRepository;

    public ReservatieManager(IReservatieRepository reservatieRepository, IRestaurantRepository restaurantRepository, IGebruikerRepository gebruikerRepository, ITafelRepository tafelRepository, ILocatieRepository locatieRepository)
    {
        _reservatieRepository = reservatieRepository;
        _restaurantRepository = restaurantRepository;
        _gebruikerRepository = gebruikerRepository;
        _tafelRepository = tafelRepository;
        _locatieRepository = locatieRepository;
    }

    // WERKT
    public void VoegReservatieToe(Reservatie reservatie)
    {
        if (reservatie == null) throw new ReservatieManagerException("VoegReservatieToe - Reservatie is null");
        try
        {
            if (_reservatieRepository.BestaatReservatie(reservatie)) throw new ReservatieManagerException("VoegReservatieToe - Reservatie bestaat al");
            if (!_locatieRepository.BestaatLocatie(reservatie.Gebruiker.Locatie)) throw new ReservatieManagerException("VoegReservatieToe - Gebruiker locatie bestaat niet");

            if (!_gebruikerRepository.BestaatGebruiker(reservatie.Gebruiker)) throw new ReservatieManagerException("VoegReservatieToe - Gebruiker bestaat niet");
            Gebruiker g = _gebruikerRepository.GeefGebruiker(reservatie.Gebruiker.Id);
            
            if (!_restaurantRepository.BestaatRestaurant(reservatie.Restaurant)) throw new ReservatieManagerException("VoegReservatieToe - Restaurant bestaat niet");
            Restaurant r = _restaurantRepository.GeefRestaurant(reservatie.Restaurant.Id);
            
            if (!_tafelRepository.BestaatTafel(reservatie.Tafel.Tafelnummer, r)) throw new ReservatieManagerException("VoegReservatieToe - Tafel bestaat niet");
            
            _reservatieRepository.VoegReservatieToe(reservatie);
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("VoegReservatieToe - Reservatie kon niet toegevoegd worden", ex);
        }
    }

    public void DeleteReservatie(Reservatie reservatie)
    {
        if (reservatie == null) throw new ReservatieManagerException("Reservatie is niet ingevuld");
        try
        {
            if (!_reservatieRepository.BestaatReservatie(reservatie))
                throw new ReservatieManagerException("Reservatie bestaat niet");
            _reservatieRepository.DeleteReservatie(reservatie);
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("Reservatie kon niet verwijderd worden", ex);
        }
    }

    public void UpdateReservatie(Reservatie reservatie)
    {
        if (reservatie == null) throw new ReservatieManagerException("Reservatie is niet ingevuld");
        try
        {
            if (!_reservatieRepository.BestaatReservatie(reservatie)) throw new ReservatieManagerException("Reservatie bestaat niet");
            if (reservatie.Gebruiker == null) throw new ReservatieManagerException("Gebruiker is niet ingevuld");
            if (reservatie.Restaurant == null) throw new ReservatieManagerException("Restaurant is niet ingevuld");
            //TODO Enkel datum/uur/aantalplaatsen aanpasbaar maken  

            _reservatieRepository.UpdateReservatie(reservatie);
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("Reservatie kon niet geupdate worden", ex);
        }
    }

    public Reservatie GeefReservatie(int id)
    {
        if (id < 0) throw new ReservatieManagerException("Id is kleiner dan 0");
        try
        {
            return _reservatieRepository.GeefReservatie(id);
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("Reservatie kon niet gevonden worden", ex);
        }
    }
}