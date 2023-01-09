using ReservatieServiceBL.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ReservatieServiceBL.Entities;

public partial class Gebruiker
{
    public Gebruiker(string naam, string email, string telefoonnummer)
    {
        ZetNaam(naam);
        ZetEmail(email);
        ZetTelefoonnummer(telefoonnummer);
    }

    public Gebruiker(int gebruikerId, string naam, string email, string telefoonnummer)
    {
        GebruikerId = gebruikerId;
        Naam = naam;
        Email = email;
        Telefoonnummer = telefoonnummer;
    }

    public Gebruiker()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GebruikerId { get; set; }

    public string Naam { get; set; }

    public string Email { get; set; } = null!;

    public string Telefoonnummer { get; set; } = null!;

    public int LocatieId { get; set; }

    public int IsVisible { get; set; }

    public virtual Locatie Locatie { get; set; } = null!;

    public virtual ICollection<Reservatie> Reservaties { get; } = new List<Reservatie>();

    public void ZetGebruikerId(int id)
    {
        if (id <= 0) throw new GebruikerException("ZetGebruikerId - Id <= 0");
        GebruikerId = id;
    }
    public void ZetNaam(string naam)
    {
        if (string.IsNullOrWhiteSpace(naam)) throw new GebruikerException("ZetNaam - null/whitespace");
        Naam = naam;
    }

    public void ZetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new GebruikerException("ZetEmail - null/whitespace");
        if (!IsEmail(email)) throw new GebruikerException("ZetEmail - geen geldig emailadres");
        Email = email;
    }
    public void ZetTelefoonnummer(string telefoonnummer)
    {
        var regex = @"^(((\+|00)32[ ]?(?:\(0\)[ ]?)?)|0){1}(4(60|[789]\d)\/?(\s?\d{2}\.?){2}(\s?\d{2})|(\d\/?\s?\d{3}|\d{2}\/?\s?\d{2})(\.?\s?\d{2}){2})$";
        if (string.IsNullOrWhiteSpace(telefoonnummer)) throw new GebruikerException("ZetTelefoonnummer - null/whitespace");
        if (!Regex.IsMatch(telefoonnummer, regex)) throw new GebruikerException("ZetTelefoonnr - geen geldig telefoonnummer");
        Telefoonnummer = telefoonnummer;
    }
    public void ZetLocatie(Locatie locatie)
    {
        if (locatie == null) throw new GebruikerException("ZetLocatie - null");
        Locatie = locatie;
        LocatieId = Locatie.LocatieId;
    }
    public void ZetLocatieId()
    {
        if (Locatie != null) LocatieId = Locatie.LocatieId;
    }

    public void VoegReservatieToe(Reservatie reservatie)
    {
        if (reservatie == null) throw new GebruikerException("VoegReservatieToe - null");
        if (Reservaties.Contains(reservatie)) throw new GebruikerException("VoegReservatieToe - reservatie bestaat al");
        Reservaties.Add(reservatie);
    }

    public void VerwijderReservatie(Reservatie reservatie)
    {
        if (reservatie == null) throw new GebruikerException("VerwijderReservatie - null");
        if (!Reservaties.Contains(reservatie)) throw new RestaurantException("VerwijderReservatie - reservatie bestaat niet");
        Reservaties.Remove(reservatie);
    }
    public bool IsDezelfde(Gebruiker gebruiker)
    {
        if (gebruiker == null) throw new GebruikerException("IsDezelfde - null");
        if (!Naam.Equals(gebruiker.Naam)) return false;
        if (!Email.Equals(gebruiker.Email)) return false;
        if (!Telefoonnummer.Equals(gebruiker.Telefoonnummer)) return false;
        if (!Locatie.IsDezelfde(gebruiker.Locatie)) return false;
        return true;
    }

    private bool IsEmail(string email)
    {
        try
        {
            MailAddress mail = new(email);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is Gebruiker gebruiker &&
               GebruikerId == gebruiker.GebruikerId;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GebruikerId);
    }
}
