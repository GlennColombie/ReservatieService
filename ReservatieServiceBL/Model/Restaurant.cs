using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservatieServiceBL.Exceptions;

namespace ReservatieServiceBL.Model
{
    public class Restaurant
    {
        public Restaurant(string naam, Locatie locatie, string telefoonnr, string email, Keuken keuken)
        {
            ZetNaam(naam);
            ZetLocatie(locatie);
            ZetTelefoon(telefoonnr);
            ZetEmail(email);
            ZetKeuken(keuken);
        }
        
        public int Id { get; set; }
        public string Naam { get; private set; }
        public Locatie Locatie { get; private set; }
        public string Telefoon { get; private set; }
        public string Email { get; private set; }
        public Keuken Keuken { get; private set; }

        private List<Reservatie> _reservaties = new();

        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new RestaurantException("Naam mag niet leeg zijn");
            Naam = naam;
        }
        
        public void ZetLocatie(Locatie locatie)
        {
            if (locatie == null) throw new RestaurantException("Locatie mag niet leeg zijn");
            Locatie = locatie;
        }
        
        public void ZetTelefoon(string telefoon)
        {
            if (string.IsNullOrWhiteSpace(telefoon)) throw new RestaurantException("Telefoon mag niet leeg zijn");
            Telefoon = telefoon;
        }
        
        public void ZetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new RestaurantException("Email mag niet leeg zijn");
            Email = email;
        }
        
        public void ZetKeuken(Keuken keuken)
        {
            Keuken = keuken;
        }

        public IReadOnlyList<Reservatie> GeefReservaties()
        {
            return _reservaties.AsReadOnly();
        }

        public void VerwijderReservatie(Reservatie reservatie)
        {
            if (reservatie == null) throw new RestaurantException("Reservatie mag niet leeg zijn");
            if (!_reservaties.Contains(reservatie)) throw new RestaurantException("Reservatie bestaat niet");
            _reservaties.Remove(reservatie);
            reservatie.VerwijderRestaurant();
        }

        public void VoegReservatieToe(Reservatie reservatie)
        {
            if (reservatie == null) throw new RestaurantException("Reservatie mag niet leeg zijn");
            if (_reservaties.Contains(reservatie)) throw new RestaurantException("Reservatie bestaat al");
            _reservaties.Add(reservatie);
            if (reservatie.Restaurant != this || reservatie.Restaurant == null) reservatie.ZetRestaurant(this);
        }
    }
}