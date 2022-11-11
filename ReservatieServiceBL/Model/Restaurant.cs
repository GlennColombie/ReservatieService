using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ReservatieServiceBL.Exceptions;

namespace ReservatieServiceBL.Model
{
    public class Restaurant
    {
        [JsonConstructor]
        public Restaurant (int id, string naam, Locatie locatie, string telefoonnummer, string email, Keuken keuken)
        {
            ZetId(id);
            ZetNaam(naam);
            ZetLocatie(locatie);
            ZetTelefoonnummer(telefoonnummer);
            ZetEmail(email);
            ZetKeuken(keuken);
        }
        public Restaurant(string naam, Locatie locatie, string telefoonnummer, string email, Keuken keuken)
        {
            ZetNaam(naam);
            ZetLocatie(locatie);
            ZetTelefoonnummer(telefoonnummer);
            ZetEmail(email);
            ZetKeuken(keuken);
        }
        
        public int Id { get; set; }
        public string Naam { get; private set; }
        public Locatie Locatie { get; private set; }
        public string Telefoonnummer { get; private set; }
        public string Email { get; private set; }
        public Keuken Keuken { get; private set; }

        private List<Reservatie> _reservaties = new();

        public void ZetId(int id)
        {
            if (id < 0) throw new RestaurantException("Id < 0");
            Id = id;
        }
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
        
        public void ZetTelefoonnummer(string telefoon)
        {
            if (string.IsNullOrWhiteSpace(telefoon)) throw new RestaurantException("Telefoonnummer mag niet leeg zijn");
            Telefoonnummer = telefoon;
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
        
        public bool IsHetzelfde(Restaurant restaurant)
        {
            if (restaurant == null) throw new RestaurantException("IsHetzelfde - null");
            if (!restaurant.Naam.Equals(Naam)) return false;
            if (!restaurant.Locatie.IsDezelfde(Locatie)) return false;
            if (!restaurant.Telefoonnummer.Equals(Telefoonnummer)) return false;
            if (!restaurant.Email.Equals(Email)) return false;
            if (!restaurant.Keuken.ToString().Equals(Keuken)) return false;
            return true;
            {
            }
        }
    }
}