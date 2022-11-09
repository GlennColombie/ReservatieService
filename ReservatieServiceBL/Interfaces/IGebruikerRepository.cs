using ReservatieServiceBL.Model;

namespace ReservatieServiceBL.Interfaces;

public interface IGebruikerRepository
{
    void GebruikerRegistreren(Gebruiker gebruiker);
    void UpdateGebruiker(Gebruiker gebruiker);
    void VerwijderGebruiker(Gebruiker gebruiker);

    bool BestaatGebruiker(Gebruiker gebruiker);
    bool BestaatGebruiker(int gebruiker);
    Gebruiker GeefGebruiker(int id);

    IReadOnlyList<Reservatie> ZoekReservaties(DateTime? begindatum, DateTime? einddatum);
    IReadOnlyList<Reservatie> GeefReservaties(DateTime? begindatum, DateTime? einddatum);
    IReadOnlyList<Gebruiker> GeefGebruikers();
}