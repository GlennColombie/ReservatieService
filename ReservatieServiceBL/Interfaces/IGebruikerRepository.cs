using Microsoft.EntityFrameworkCore;
using ReservatieServiceBL.Entities;

namespace ReservatieServiceBL.Interfaces;

public interface IGebruikerRepository
{
    void GebruikerRegistreren(Gebruiker gebruiker);
    void UpdateGebruiker(Gebruiker gebruiker);
    void VerwijderGebruiker(Gebruiker gebruiker);
    bool BestaatGebruiker(Gebruiker gebruiker);
    bool BestaatGebruiker(int gebruiker);
    Gebruiker GeefGebruiker(int id);
    IReadOnlyList<Reservatie> GeefReservaties(Gebruiker gebruiker, DateTime? begindatum, DateTime? einddatum);
    IReadOnlyList<Gebruiker> GeefGebruikers();
    Gebruiker GeefGebruiker(string email);
}