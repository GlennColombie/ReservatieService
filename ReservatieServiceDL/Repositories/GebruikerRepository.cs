using System.Data.SqlClient;
using System.IO;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Entities;
using ReservatieServiceDL.Exceptions;
using ReservatieServiceDL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ReservatieServiceDL.Repositories;

public class GebruikerRepository : IGebruikerRepository
{
    private readonly ReservatieServiceContext _context;

    public GebruikerRepository(string connectionString)
    {
        _context = new(connectionString);
    }

    private void SaveAndClear()
    {
        _context.SaveChanges();
        _context.ChangeTracker.Clear();
    }
    public void GebruikerRegistreren(Gebruiker gebruiker)
    {
        if (gebruiker == null) throw new GebruikerRepositoryException("GebruikerRegistreren - null");
        try
        {
            _context.Gebruikers.Add(gebruiker);
            _context.Entry(gebruiker.Locatie).State = EntityState.Unchanged;
            SaveAndClear();
        }
        catch (Exception ex)
        {
            throw new GebruikerRepositoryException("GebruikerRegistreren - repo", ex);
        }
    }

    public void UpdateGebruiker(Gebruiker gebruiker)
    {
        var gebruikerInDb = _context.Gebruikers.Find(gebruiker.GebruikerId);
        gebruikerInDb.ZetNaam(gebruiker.Naam);
        gebruikerInDb.ZetEmail(gebruiker.Email);
        gebruikerInDb.ZetTelefoonnummer(gebruiker.Telefoonnummer);
        _context.Gebruikers.Update(gebruikerInDb);
        SaveAndClear();
    }

    public void VerwijderGebruiker(Gebruiker gebruiker)
    {
        _context.Gebruikers.Remove(gebruiker);
        SaveAndClear();
    }

    public bool BestaatGebruiker(Gebruiker gebruiker)
    {
        try
        {
            return _context.Gebruikers.Any(g => g.Naam == gebruiker.Naam && g.Email == gebruiker.Email && g.Telefoonnummer == gebruiker.Telefoonnummer && g.Locatie == gebruiker.Locatie);
        }
        catch (Exception ex)
        {
            throw new GebruikerRepositoryException("BestaatGebruiker - repo", ex);
        }
    }

    public bool BestaatGebruiker(int id)
    {
        return _context.Gebruikers.Any(g => g.GebruikerId == id);
    }

    public Gebruiker GeefGebruiker(int id)
    {
        try
        {
            return _context.Gebruikers.Where(g => g.GebruikerId == id).Include(g => g.Locatie).Include(g => g.Reservaties).ThenInclude(r => r.Tafel).Include(g => g.Reservaties).ThenInclude(r => r.Restaurant.Locatie).AsNoTracking().FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new GebruikerRepositoryException("GeefGebruiker - repo", ex);
        }
    }

    public IReadOnlyList<Reservatie> GeefReservaties(Gebruiker gebruiker, DateTime? begindatum, DateTime? einddatum)
    {
        try
        {
            return _context.Reservaties.Where(r => r.Gebruiker == gebruiker && r.Datum >= begindatum && r.Datum <= einddatum).Include(r => r.Restaurant.Locatie).Include(r => r.Tafel).AsNoTracking().ToList();
        }
        catch (Exception ex)
        {
            throw new GebruikerRepositoryException("GeefReservaties - repo", ex);
        }
    }

    public IReadOnlyList<Gebruiker> GeefGebruikers()
    {
        try
        {
            return _context.Gebruikers.Include(g => g.Locatie).Include(g => g.Reservaties).AsNoTracking().ToList();
        }
        catch (Exception ex)
        {
            throw new GebruikerRepositoryException("GeefGebruikers - repo", ex);
        }
    }
}