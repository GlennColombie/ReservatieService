using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;

namespace ReservatieServiceBL.Managers;

public class ReservatieManager
{
    private IReservatieRepository _reservatieRepository;

    public ReservatieManager(IReservatieRepository reservatieRepository)
    {
        _reservatieRepository = reservatieRepository;
    }

    public void AddReservatie(Reservatie reservatie)
    {
        if (reservatie == null) throw new ArgumentNullException(nameof(reservatie));
        try
        {
            if (_reservatieRepository.BestaatReservatie(reservatie)) throw new Exception("Reservatie bestaat al");
            if (reservatie.Gebruiker == null) throw new Exception("Gebruiker is niet ingevuld");
            if (reservatie.Restaurant == null) throw new Exception("Restaurant is niet ingevuld");
            _reservatieRepository.AddReservatie(reservatie);
        }
        catch (Exception ex)
        {
            throw new ReservatieManagerException("Reservatie kon niet toegevoegd worden", ex);
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