using ReservatieServiceBL.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservatieServiceBL.Entities;

public partial class Tafel
{
    public Tafel(int tafelnummer, int aantalPlaatsen)
    {
        ZetTafelnummer(tafelnummer);
        ZetAantalPlaatsen(aantalPlaatsen);
    }

    public Tafel()
    {
    }

    public int Tafelnummer { get; set; }

    public int AantalPlaatsen { get; set; }

    public int RestaurantId { get; set; }

    public int IsVisible { get; set; }

    public virtual Restaurant Restaurant { get; set; } = null!;

    public virtual ICollection<Reservatie> Reservaties { get; } = new List<Reservatie>();

    public void ZetTafelnummer(int tafelnummer)
    {
        if (tafelnummer < 0) throw new TafelException("ZetTafelnummer - tafelnummer < 0");
        Tafelnummer = tafelnummer;
    }

    public void ZetAantalPlaatsen(int aantalPlaatsen)
    {
        if (aantalPlaatsen < 0) throw new TafelException("ZetAantalPlaatsen - aantalPlaatsen < 0");
        AantalPlaatsen = aantalPlaatsen;
    }
    
    public void ZetRestaurantId()
    {
        RestaurantId = Restaurant.Id;
    }

    public bool IsDezelfde(Tafel tafel)
    {
        if (tafel.Tafelnummer != Tafelnummer) return false;
        if (tafel.AantalPlaatsen != AantalPlaatsen) return false;
        if (tafel.Restaurant != Restaurant) return false;
        return true;
    }
}
