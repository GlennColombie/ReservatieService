using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Entities;
using System.Globalization;

namespace ReservatieServiceBL.Managers;

public class GebruikerManager
{
    string[] dateFormats = { "dd/MM/yyyy", "dd-MM-yyyy", "dd.MM.yyyy", "dd/MM/yy", "dd-MM-yy", "dd.MM.yy" };
    private IGebruikerRepository _gebruikerRepository;
    private ILocatieRepository _locatieRepository;

    public GebruikerManager(IGebruikerRepository gebruikerRepository, ILocatieRepository locatieRepository)
    {
        _gebruikerRepository = gebruikerRepository;
        _locatieRepository = locatieRepository;
    }

    public GebruikerManager()
    {
    }

    public virtual void GebruikerRegistreren(Gebruiker gebruiker)
    {
        if (gebruiker == null) throw new GebruikerManagerException("Gebruiker is null");
        try
        {
            if (!_locatieRepository.BestaatLocatie(gebruiker.Locatie)) _locatieRepository.VoegLocatieToe(gebruiker.Locatie);
            Locatie l = _locatieRepository.GeefLocatie(gebruiker.Locatie);
            if (_gebruikerRepository.BestaatGebruiker(gebruiker)) throw new GebruikerManagerException("Gebruiker bestaat al");
            gebruiker.ZetLocatieId();
            _gebruikerRepository.GebruikerRegistreren(gebruiker);
        }
        catch (Exception ex)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het toevoegen van de gebruiker", ex);
        }
    }

    public virtual void GebruikerUpdaten(Gebruiker gebruiker)
    {
        if (gebruiker == null) throw new GebruikerManagerException("Gebruiker is null");
        try
        {
            if (_gebruikerRepository.BestaatGebruiker(gebruiker.GebruikerId))
            {
                Gebruiker g = _gebruikerRepository.GeefGebruiker(gebruiker.GebruikerId);
                if (g.IsDezelfde(gebruiker)) throw new GebruikerManagerException("Gebruiker is hetzelfde");
                if (!_locatieRepository.BestaatLocatie(gebruiker.Locatie)) _locatieRepository.VoegLocatieToe(gebruiker.Locatie);
                Locatie l = _locatieRepository.GeefLocatie(gebruiker.Locatie);
                gebruiker.Locatie = l;
                gebruiker.ZetLocatieId();
                _gebruikerRepository.UpdateGebruiker(gebruiker);
            }
            else throw new GebruikerManagerException("Gebruiker bestaat niet");
        }
        catch (Exception ex)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het updaten van de gebruiker", ex);
        }
    }

    public virtual void GebruikerVerwijderen(Gebruiker gebruiker)
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

    public virtual bool BestaatGebruiker(int id)
    {
        if (id <= 0) throw new GebruikerManagerException("GebruikerId is kleiner of gelijk aan 0");
        try
        {
            return _gebruikerRepository.BestaatGebruiker(id);
        }
        catch (Exception ex)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het controleren of de gebruiker bestaat", ex);
        }
    }

    public virtual Gebruiker GeefGebruiker(int klantnr)
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

    public virtual Gebruiker GeefGebruiker(string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email)) throw new GebruikerManagerException("Gebruikersnaam is null of leeg");
            return _gebruikerRepository.GeefGebruiker(email);
        }
        catch (Exception ex)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het ophalen van de gebruiker", ex);
        }
    }

    public virtual IReadOnlyList<Gebruiker> GeefGebruikers()
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

    public virtual IReadOnlyList<Reservatie> ZoekReservaties(Gebruiker gebruiker, string? begindatum, string? einddatum)
    {
        try
        {
            if (gebruiker == null) throw new GebruikerManagerException("GeefReservaties - gebruiker is null");

            if (!string.IsNullOrWhiteSpace(begindatum) && !string.IsNullOrWhiteSpace(einddatum))
            {
                DateTime begin = DateTime.Parse(begindatum);
                DateTime eind = DateTime.Parse(einddatum);
                return _gebruikerRepository.GeefReservaties(gebruiker, begin, eind);
            }
            else if (!string.IsNullOrWhiteSpace(einddatum))
            {
                DateTime eind = DateTime.Parse(einddatum);
                return _gebruikerRepository.GeefReservaties(gebruiker, DateTime.Parse("1/1/1900"), eind);
            }
            else if (!string.IsNullOrWhiteSpace(begindatum))
            {
                DateTime begin = DateTime.Parse(begindatum);
                return _gebruikerRepository.GeefReservaties(gebruiker, begin, DateTime.MaxValue);
            }
            else
            {
                return _gebruikerRepository.GeefReservaties(gebruiker, DateTime.Parse("1/1/1900"), DateTime.MaxValue);
            }
        }
        catch (Exception ex)
        {
            throw new GebruikerManagerException("Er is een fout opgetreden bij het ophalen van de reservaties", ex);
        }
    }
}