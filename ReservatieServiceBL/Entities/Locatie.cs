using ReservatieServiceBL.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservatieServiceBL.Entities;

public partial class Locatie
{
    public Locatie(int postcode, string gemeente, string? straat, string? huisnummer)
    {
        ZetPostcode(postcode);
        ZetGemeente(gemeente);
        ZetStraat(straat);
        ZetHuisnummer(huisnummer);
    }

    public Locatie(int locatieId, int postcode, string gemeente, string? straat, string? huisnummer, int isVisible, ICollection<Gebruiker> gebruikers, ICollection<Restaurant> restaurants)
    {
        ZetLocatieId(locatieId);
        ZetPostcode(postcode);
        ZetGemeente(gemeente);
        ZetStraat(straat);
        ZetHuisnummer(huisnummer);
        IsVisible = isVisible;
        Gebruikers = gebruikers;
        Restaurants = restaurants;
    }

    public Locatie()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LocatieId { get; set; }

    public int Postcode { get; set; }

    public string Gemeente { get; set; } = null!;

    public string? Straat { get; set; }

    public string? Huisnummer { get; set; }

    public int IsVisible { get; set; }

    public virtual ICollection<Gebruiker> Gebruikers { get; } = new List<Gebruiker>();

    public virtual ICollection<Restaurant> Restaurants { get; } = new List<Restaurant>();

    public void ZetLocatieId(int id)
    {
        if (id <= 0) throw new LocatieException("LocatieId mag niet kleiner zijn dan 0");
        LocatieId = id;
    }
    public void ZetPostcode(int postcode)
    {
        if (postcode < 1000 || postcode > 9999) throw new LocatieException("ZetPostcode - niet geldig");
        Postcode = postcode;
    }

    public void ZetGemeente(string gemeente)
    {
        if (string.IsNullOrWhiteSpace(gemeente)) throw new LocatieException("ZetGemeente - null");
        Gemeente = gemeente;
    }

    public void ZetStraat(string? straat)
    {
        Straat = straat;
    }

    public void ZetHuisnummer(string? huisnummer)
    {
        Huisnummer = huisnummer;
    }

    public bool IsDezelfde(Locatie locatie)
    {
        if (locatie == null) throw new LocatieException("IsDezelfde - Locatie is null");
        if (locatie.LocatieId != LocatieId) return false;
        if (!locatie.Postcode.Equals(Postcode)) return false;
        if (!locatie.Gemeente.Equals(Gemeente)) return false;
        return true;
    }
}
