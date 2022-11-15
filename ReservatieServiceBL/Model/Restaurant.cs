using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ReservatieServiceBL.Exceptions;

namespace ReservatieServiceBL.Model
{
    public class Restaurant
    {
        [JsonConstructor]
        public Restaurant(int id, string naam, Locatie locatie, string telefoonnummer, string email, Keuken keuken)
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
        public List<Tafel> Tafels { get => _tafels; private set => _tafels = value; }
        public List<Reservatie> Reservaties { get => _reservaties; private set => _reservaties = value; }

        private List<Reservatie> _reservaties = new();

        private List<Tafel> _tafels = new();

        #region Setters
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
            var regex = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
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

        #endregion

        #region Tafels


        public void VoegTafelToe(Tafel tafel)
        {
            if (tafel == null) throw new RestaurantException("Tafel mag niet leeg zijn");
            if (_tafels.Contains(tafel)) throw new RestaurantException("Tafel bestaat al");
            _tafels.Add(tafel);
            //if ((tafel.Restaurant == null) || (tafel.Restaurant != this)) tafel.ZetRestaurant(this);
        }

        public void VerwijderTafel(Tafel tafel)
        {
            if (tafel == null) throw new RestaurantException("Tafel mag niet leeg zijn");
            if (!_tafels.Contains(tafel)) throw new RestaurantException("Tafel bestaat niet");
            _tafels.Remove(tafel);
            //tafel.VerwijderRestaurant();
        }

        public void UpdateTafel(Tafel tafel)
        {
            if (tafel == null) throw new RestaurantException("Tafel mag niet leeg zijn");
            if (_tafels.Any(t => t.Tafelnummer == tafel.Tafelnummer))
            {
                _tafels.Remove(_tafels.First(t => t.Tafelnummer == tafel.Tafelnummer));
                _tafels.Add(tafel);
            }
        }
        public IReadOnlyList<Tafel> GeefTafels()
        {
            return _tafels.AsReadOnly();
        }

        #endregion

        #region Reservaties

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

        public void UpdateReservatie(Reservatie reservatie)
        {
            if (reservatie == null) throw new RestaurantException("Reservatie mag niet leeg zijn");
            if (_reservaties.Any(r => r.Reservatienummer == reservatie.Reservatienummer))
            {
                _reservaties.Remove(_reservaties.First(r => r.Reservatienummer == reservatie.Reservatienummer));
                _reservaties.Add(reservatie);
            }
        }
        public IReadOnlyList<Reservatie> GeefReservaties()
        {
            return _reservaties.AsReadOnly();
        }

        #endregion
        
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

        public override bool Equals(object? obj)
        {
            return obj is Restaurant restaurant &&
                   Id == restaurant.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
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
                return false;
            }
        }
    }
}