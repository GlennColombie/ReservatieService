using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Entities;
using System.Globalization;

namespace ReservatieServiceBL.Managers;

public class ReservatieManager
{
    private IReservatieRepository _reservatieRepository;
    private IRestaurantRepository _restaurantRepository;
    private IGebruikerRepository _gebruikerRepository;
    private ILocatieRepository _locatieRepository;
    public ReservatieManager(IReservatieRepository reservatieRepository, IRestaurantRepository restaurantRepository, IGebruikerRepository gebruikerRepository, ILocatieRepository locatieRepository)
    {
        _reservatieRepository = reservatieRepository;
        _restaurantRepository = restaurantRepository;
        _gebruikerRepository = gebruikerRepository;
        _locatieRepository = locatieRepository;
    }

    public ReservatieManager()
    {
    }

    public virtual void VoegReservatieToe(Reservatie reservatie)
    {
        if (reservatie == null) throw new ReservatieManagerException("VoegReservatieToe - Reservatie is null");
        try
        {
            if (_reservatieRepository.BestaatReservatie(reservatie)) throw new ReservatieManagerException("VoegReservatieToe - Reservatie bestaat al");
            _reservatieRepository.VoegReservatieToe(reservatie);
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("VoegReservatieToe - Reservatie kon niet toegevoegd worden", ex);
        }
    }

    public virtual void AnnuleerReservatie(int reservatieNummer)
    {
        if (reservatieNummer <= 0) throw new ReservatieManagerException("AnnuleerReservatie - ReservatieNummer is kleiner of gelijk aan 0");
        try
        {
            Reservatie r = GeefReservatie(reservatieNummer);
            if (r == null) throw new ReservatieManagerException("AnnuleerReservatie - Bestaat niet");
            if (r.Uur < DateTime.Now) throw new ReservatieManagerException("AnnuleerReservatie - Reservatie is al verstreken");
             _reservatieRepository.AnnuleerReservatie(reservatieNummer);
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("AnnuleerReservatie", ex);
        }
    }

    public virtual void UpdateReservatie(Reservatie reservatie)
    {
        if (reservatie == null) throw new ReservatieManagerException("Reservatie is niet ingevuld");
        try
        {
            if ( _reservatieRepository.BestaatReservatie(reservatie)) throw new ReservatieManagerException("UpdateReservatie - bestaat niet");
            Reservatie r = _reservatieRepository.GeefReservatie(reservatie.Reservatienummer);
            if (r.IsDezelfde(reservatie)) throw new ReservatieManagerException("Reservatie is niet gewijzigd");
            if (reservatie.AantalPlaatsen > r.Tafel.AantalPlaatsen) throw new ReservatieManagerException("Aantal plaatsen is groter dan aantal plaatsen van tafel");
            _reservatieRepository.UpdateReservatie(reservatie);
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("UpdateReservatie", ex);
        }
    }

    public virtual Reservatie GeefReservatie(int reservatienummer)
    {
        if (reservatienummer < 0) throw new ReservatieManagerException("Id is kleiner dan 0");
        try
        {
            return _reservatieRepository.GeefReservatie(reservatienummer);
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("Reservatie kon niet gevonden worden", ex);
        }
    }
}