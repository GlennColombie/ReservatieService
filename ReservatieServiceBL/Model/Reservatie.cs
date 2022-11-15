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
        public Reservatie(int reservatienummer, Restaurant restaurant, Gebruiker gebruiker, Tafel tafel,
            DateTime datum, DateTime uur)
        {
            ZetReservatienummer(reservatienummer);
            ZetRestaurant(restaurant);
            ZetGebruiker(gebruiker);
            ZetTafel(tafel);
            //ZetAantalPlaatsen(aantalPlaatsen);
            Datum = datum;
            Uur = uur;
            //ZetTafelnummer(tafelnummer);
        }

        public int Reservatienummer { get; private set; }
        public Restaurant Restaurant { get; private set; }
        public Gebruiker Gebruiker { get; private set; }
        //public int AantalPlaatsen { get; private set; }
        public Tafel Tafel { get; private set; }
        public DateTime Datum { get; set; }
        public DateTime Uur { get; set; }
        public DateTime Einduur => Uur.AddHours(1.5);

        //public int Tafelnummer { get; private set; }
        
        //public string Tijd { get; set; }

        public void ZetReservatienummer(int nr)
        {
            if (nr < 0) throw new ReservatieException("ZetReservatieNummer - Reservatienummer moet groter zijn dan 0");
            Reservatienummer = nr;
        }

        public void ZetRestaurant(Restaurant restaurant)
        {
            if (restaurant == null) throw new ReservatieException("ZetRestaurant - null");
            if (restaurant.Equals(Restaurant)) throw new ReservatieException("ZetRestaurant - Hetzelfde restaurant");
            if (Restaurant != null) Restaurant.VerwijderReservatie(this);
            Restaurant = restaurant;
            if (!Restaurant.GeefReservaties().Contains(this)) restaurant.VoegReservatieToe(this);
        }
        
        public void ZetGebruiker(Gebruiker gebruiker)
        {
            if (gebruiker == null) throw new ReservatieException("ZetGebruiker - null");
            if (gebruiker.Equals(Gebruiker)) throw new ReservatieException("ZetGebruiker - Dezelfde gebruiker");
            if (Gebruiker != null) Gebruiker.VerwijderReservatie(this);
            Gebruiker = gebruiker;
            if (!gebruiker.GeefReservaties().Contains(this)) gebruiker.VoegReservatieToe(this);
        }

        //public void ZetAantalPlaatsen(int aantal)
        //{
        //    if (aantal < 0) throw new ReservatieException("ZetAantalPlaatsen - Aantal plaatsen moet groter zijn dan 0");
        //    AantalPlaatsen = aantal;
        //}

        //public void ZetTafelnummer(int tafelnummer)
        //{
        //    if (tafelnummer < 0) throw new ReservatieException("ZetTafelNummer - Tafelnummer moet groter zijn dan 0");
        //    Tafelnummer = tafelnummer;
        //}

        public void ZetTafel(Tafel tafel)
        {
            if (tafel == null) throw new ReservatieException("ZetTafel - null");
            if (tafel.Equals(Tafel)) throw new ReservatieException("ZetTafel - Dezelfde tafel");
            Tafel = tafel;
        }

        public void ZetDatum(DateTime datum)
        {
            if (datum < DateTime.Now) throw new ReservatieException("ZetDatum - Datum moet groter of gelijk zijn aan vandaag");
            Datum = datum;
        }
        
        public void ZetUur(DateTime uur)
        {
            if (uur < DateTime.Now) throw new ReservatieException("ZetUur - Uur moet groter zijn dan nu");
            Uur = uur;
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
