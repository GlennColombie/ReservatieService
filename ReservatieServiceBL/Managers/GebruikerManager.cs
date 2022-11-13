using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;

namespace ReservatieServiceBL.Managers;

public class GebruikerManager
{
    private IGebruikerRepository _gebruikerRepository;
    private ILocatieRepository _locatieRepository;

    public GebruikerManager(IGebruikerRepository gebruikerRepository, ILocatieRepository locatieRepository)
    {
        _gebruikerRepository = gebruikerRepository;
        _locatieRepository = locatieRepository;
    }

    public void GebruikerRegistreren(Gebruiker gebruiker)
    {
        if (gebruiker == null) throw new GebruikerManagerException("Gebruiker is null");
        try
        {
            //if (_gebruikerRepository.BestaatGebruiker(gebruiker)) throw new GebruikerManagerException("Gebruiker bestaat al");
            //if (!_locatieRepository.BestaatLocatie(gebruiker.Locatie)) _locatieRepository.VoegLocatieToe(gebruiker.Locatie);
            //gebruiker.ZetLocatie(_locatieRepository.GeefLocatie(gebruiker.Locatie));
            //_gebruikerRepository.GebruikerRegistreren(gebruiker);
        }
        catch (Exception ex)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het toevoegen van de gebruiker", ex);
        }
    }

    public void GebruikerUpdaten(Gebruiker gebruiker)
    {
        if (gebruiker == null) throw new GebruikerManagerException("Gebruiker is null");
        try
        {
            if (_gebruikerRepository.BestaatGebruiker(gebruiker.Id))
            {
                Gebruiker g = _gebruikerRepository.GeefGebruiker(gebruiker.Id);
                if (g.IsDezelfde(gebruiker)) throw new GebruikerManagerException("Gebruiker is hetzelfde");
                if (!_locatieRepository.BestaatLocatie(gebruiker.Locatie)) _locatieRepository.VoegLocatieToe(gebruiker.Locatie);
                _gebruikerRepository.UpdateGebruiker(gebruiker);
            }
            else throw new GebruikerManagerException("Gebruiker bestaat niet");
        }
        catch (Exception ex)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het updaten van de gebruiker", ex);
        }
    }

    public void GebruikerVerwijderen(Gebruiker gebruiker)
    {
        if (gebruiker == null) throw new GebruikerManagerException("Gebruiker is null");
        try
        {
            if (!_gebruikerRepository.BestaatGebruiker(gebruiker)) throw new GebruikerManagerException("Gebruiker bestaat niet");
            _gebruikerRepository.VerwijderGebruiker(gebruiker);
        }
        catch (Exception ex)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het verwijderen van de gebruiker", ex);
        }
    }

    public Gebruiker GeefGebruiker(int klantnr)
    {
        try
        {
            if (klantnr < 0) throw new GebruikerManagerException("Klantnr is kleiner dan 0");
            if (!_gebruikerRepository.BestaatGebruiker(klantnr)) throw new GebruikerManagerException("Gebruiker bestaat niet");
            return _gebruikerRepository.GeefGebruiker(klantnr);
        }
        catch (Exception ex)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het ophalen van de gebruiker", ex);
        }
    }

    public IReadOnlyList<Gebruiker> GeefGebruikers()
    {
        try
        {
            return _gebruikerRepository.GeefGebruikers();
        }
        catch (Exception ex)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het ophalen van de gebruikers", ex);
        }
    }

    public IReadOnlyList<Gebruiker> GeefBestaandeGebruikers()
    {
        try
        {
            return _gebruikerRepository.GeefBestaandeGebruikers();
        }
        catch (Exception ex)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het ophalen van de gebruikers", ex);
        }
    }

    public IReadOnlyList<Reservatie> ZoekReservaties(DateTime? begindatum, DateTime? einddatum)
    {
        try
        {
            List<Reservatie> reservaties = new();
            if (begindatum.HasValue && !einddatum.HasValue)
            {
                reservaties.AddRange(_gebruikerRepository.GeefReservaties(begindatum.Value, null));
                return reservaties;
            }
            else if (!begindatum.HasValue && einddatum.HasValue)
            {
                reservaties.AddRange(_gebruikerRepository.GeefReservaties(null, einddatum.Value));
                return reservaties;
            }
            else
            {
                reservaties.AddRange(_gebruikerRepository.GeefReservaties(begindatum.Value, einddatum.Value));
                return reservaties;
            }
        }
        catch (Exception ex)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het ophalen van de reservaties", ex);
        }
    }
}