using ReservatieServiceBL.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace ReservatieServiceBL.Entities;

public partial class Restaurant
{
    public Restaurant(string naam, string email, string telefoonnummer, Keuken keuken)
    {
        ZetNaam(naam);
        ZetEmail(email);
        ZetTelefoonnummer(telefoonnummer);
        ZetKeuken(keuken);
    }

    public Restaurant()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Naam { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Telefoonnummer { get; set; } = null!;

    public int LocatieId { get; set; }

    public Keuken Keuken { get; set; }

    public int IsVisible { get; set; }

    public virtual Locatie Locatie { get; set; } = null!;

    public virtual ICollection<Reservatie> Reservaties { get; set; } = new List<Reservatie>();

    public virtual ICollection<Tafel> Tafels { get; set; } = new List<Tafel>();

    public void ZetId(int id)
    {
        if (id < 0) throw new RestaurantException("ZetId - Id < 0");
        Id = id;
    }
    public void ZetNaam(string naam)
    {
        if (string.IsNullOrWhiteSpace(naam)) throw new RestaurantException("ZetNaam - null");
        Naam = naam;
    }

    public void ZetLocatie(Locatie locatie)
    {
        if (locatie == null) throw new RestaurantException("ZetLocatie - null");
        Locatie = locatie;
    }

    public void ZetTelefoonnummer(string telefoon)
    {
        var regex = @"^(((\+|00)32[ ]?(?:\(0\)[ ]?)?)|0){1}(4(60|[789]\d)\/?(\s?\d{2}\.?){2}(\s?\d{2})|(\d\/?\s?\d{3}|\d{2}\/?\s?\d{2})(\.?\s?\d{2}){2})$";
        if (string.IsNullOrWhiteSpace(telefoon)) throw new RestaurantException("ZetTelefoonnummer - null");
        if (!Regex.IsMatch(telefoon, regex)) throw new RestaurantException("ZetTelefoonnummer - geen geldig telefoonnummer");
        Telefoonnummer = telefoon;
    }

    public void ZetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) throw new RestaurantException("ZetEmail - null");
        if (!IsEmail(email)) throw new RestaurantException("ZetEmail - Email is niet geldig");
        Email = email;
    }

    public void ZetKeuken(Keuken keuken)
    {
        Keuken = keuken;
    }

    public void ZetLocatieId()
    {
        LocatieId = Locatie.LocatieId;
    }

    public bool IsHetzelfde(Restaurant restaurant)
    {
        if (restaurant == null) throw new RestaurantException("IsHetzelfde - null");
        if (!restaurant.Naam.Equals(Naam)) return false;
        if (!restaurant.Locatie.IsDezelfde(Locatie)) return false;
        if (!restaurant.Telefoonnummer.Equals(Telefoonnummer)) return false;
        if (!restaurant.Email.Equals(Email)) return false;
        if (!restaurant.Keuken.ToString().Equals(Keuken.ToString())) return false;
        return true;
    }

    private bool IsEmail(string email)
    {
        try
        {
            MailAddress mail = new(email);
            return true;
        }
        catch (Exception ex)
        {
            throw new RestaurantException("IsEmail", ex);
        }
    }
}
