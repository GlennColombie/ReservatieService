using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservatieServiceBL.Exceptions;

namespace ReservatieServiceBL.Model
{
    public class Reservatie
    {
        public Reservatie(int reservatienummer, Restaurant restaurant, Gebruiker gebruiker, int aantalPlaatsen,
            DateTime datum, DateTime uur, int tafelnummer)
        {
            ZetReservatienummer(reservatienummer);
            ZetRestaurant(restaurant);
            ZetGebruiker(gebruiker);
            ZetAantalPlaatsen(aantalPlaatsen);
            Datum = datum;
            Uur = uur;
            ZetTafelnummer(tafelnummer);
        }

        public int Reservatienummer { get; private set; }
        public Restaurant Restaurant { get; private set; }
        public Gebruiker Gebruiker { get; private set; }
        public int AantalPlaatsen { get; private set; }
        public DateTime Datum { get; set; }
        public DateTime Uur { get; set; }

        public int Tafelnummer { get; private set; }
        
        //public string Tijd { get; set; }

        public void ZetReservatienummer(int nr)
        {
            if (nr < 0) throw new ReservatieException("Reservatienummer moet groter zijn dan 0");
            Reservatienummer = nr;
        }

        public void ZetRestaurant(Restaurant restaurant)
        {
            if (restaurant == null) throw new ReservatieException("Restaurant mag niet leeg zijn");
            if (Restaurant.Equals(restaurant)) throw new ReservatieException("Hetzelfde restaurant");
            if (Restaurant != null) Restaurant.VerwijderReservatie(this);
            Restaurant = restaurant;
            if (Restaurant.GeefReservaties().Contains(this) == false) Restaurant.VoegReservatieToe(this);
        }
        
        public void ZetGebruiker(Gebruiker gebruiker)
        {
            if (gebruiker == null) throw new ReservatieException("Gebruiker mag niet leeg zijn");
            if (Gebruiker.Equals(gebruiker)) throw new ReservatieException("Dezelfde gebruiker");
            if (Gebruiker != null) Gebruiker.VerwijderReservatie(this);
            Gebruiker = gebruiker;
            if (!gebruiker.GeefReservaties().Contains(this)) gebruiker.VoegReservatieToe(this);
        }
        
        public void ZetAantalPlaatsen(int aantal)
        {
            if (aantal < 0) throw new ReservatieException("Aantal plaatsen moet groter zijn dan 0");
            AantalPlaatsen = aantal;
        }

        public void ZetTafelnummer(int tafelnummer)
        {
            if (tafelnummer < 0) throw new ReservatieException("Tafelnummer moet groter zijn dan 0");
            Tafelnummer = tafelnummer;
        }

        public void VerwijderGebruiker()
        {
            if (Gebruiker != null && Gebruiker.GeefReservaties().Contains(this)) Gebruiker.VerwijderReservatie(this);
            Gebruiker = null;
        }

        public void VerwijderRestaurant()
        {
            if (Restaurant != null && Restaurant.GeefReservaties().Contains(this)) Restaurant.VerwijderReservatie(this);
            Restaurant = null;
        }
    }
}
