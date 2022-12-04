using ReservatieServiceBL.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ReservatieServiceBL.Entities;

public partial class Reservatie
{
    string[] dateFormats = { "dd/MM/yyyy", "dd-MM-yyyy", "dd.MM.yyyy", "dd/MM/yy", "dd-MM-yy", "dd.MM.yy", "dd/MM/yyyy H:mm:ss", "dd-MM-yyyy H:mm:ss", "dd.MM.yyyy H:mm:ss", "dd/MM/yy H:mm:ss", "dd-MM-yy H:mm:ss", "dd.MM.yy H:mm:ss" };

    //public Reservatie(string datum, DateT uur, int aantalPlaatsen)
    //{
    //    ZetDatum(datum);
    //    ZetUur(uur);
    //    ZetAantalPlaatsen(aantalPlaatsen);
    //}

    public Reservatie(DateTime datum, DateTime uur, int aantalPlaatsen)
    {
        //Datum = datum;
        //Uur = uur;
        ZetDatum(datum);
        ZetUur(uur);
        AantalPlaatsen = aantalPlaatsen;
        ZetEinduur();
    }

    public Reservatie()
    {
    }

    public int Reservatienummer { get; set; }

    public int RestaurantId { get; set; }

    public int GebruikerId { get; set; }

    public int Tafelnummer { get; set; }

    public DateTime Datum { get; set; }

    public DateTime Uur { get; set; }

    public DateTime Einduur { get; set; }
    
    public int IsVisible { get; set; }

    public int AantalPlaatsen { get; set; }

    public virtual Gebruiker Gebruiker { get; set; } = null!;

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual Tafel Tafel { get; set; } = null!;

    #region Setters
    public void ZetReservatienummer(int nr)
    {
        if (nr < 0) throw new ReservatieException("ZetReservatieNummer - Reservatienummer moet groter zijn dan 0");
        Reservatienummer = nr;
    }

    public void ZetAantalPlaatsen(int aantal)
    {
        if (aantal < 0) throw new ReservatieException("ZetAantalPlaatsen - Aantal plaatsen moet groter zijn dan 0");
        AantalPlaatsen = aantal;
    }

    public void ZetDatum(DateTime datum)
    {
        if (datum < DateTime.Now) throw new ReservatieException("ZetDatum - Datum moet groter of gelijk zijn aan vandaag");
        Datum = datum;
    }

    public void ZetUur(DateTime uur)
    {
        Uur = uur;
    }

    public void ZetEinduur()
    {
        Einduur = Uur.AddHours(1.5);
    }

    public void ZetGebruikerId()
    {
        GebruikerId = Gebruiker.GebruikerId;
    }
    
    public void ZetRestaurantId()
    {
        RestaurantId = Restaurant.Id;
    }

    public void ZetTafelNummer()
    {
        Tafelnummer = Tafel.Tafelnummer;
    }

    public void ZetGebruiker(Gebruiker gebruiker)
    {
        if (gebruiker == null) throw new ReservatieException("ZetGebruiker - null");
        Gebruiker = gebruiker;
    }
    public void ZetRestaurant(Restaurant restaurant)
    {
        if (restaurant == null) throw new ReservatieException("ZetRestaurant - null");
        Restaurant = restaurant;
    }
    public void ZetTafel(Tafel tafel)
    {
        if (tafel == null) throw new ReservatieException("ZetTafel - null");
        if (tafel.Equals(Tafel)) throw new ReservatieException("ZetTafel - Dezelfde tafel");
        Tafel = tafel;
    }
    #endregion
    public bool IsDezelfde(Reservatie reservatie)
    {
        if (!reservatie.Datum.Equals(Datum)) return false;
        if (!reservatie.Uur.Equals(Uur)) return false;
        if (reservatie.AantalPlaatsen != AantalPlaatsen) return false;
        return true;
    }
}
