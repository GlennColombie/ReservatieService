using ReservatieServiceBL.Exceptions;
using System.Net.Mail;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ReservatieServiceBL.Model
{
    public class Gebruiker
    {
        public int Id { get; private set; }

        public string Naam { get; private set; }

        public string Email { get; private set; }

        public string Telefoonnummer { get; private set; }

        public Locatie Locatie { get; private set; }

        private List<Reservatie> _reservaties = new();

        [JsonConstructor]
        public Gebruiker(int id, string naam, string email, string telefoonnummer, Locatie locatie)
        {
            ZetId(id);
            ZetNaam(naam);
            ZetEmail(email);
            ZetTelefoonnummer(telefoonnummer);
            ZetLocatie(locatie);
        }
        public Gebruiker(string naam, string email, string telefoonnummer, Locatie locatie)
        {
            ZetNaam(naam);
            ZetEmail(email);
            ZetTelefoonnummer(telefoonnummer);
            ZetLocatie(locatie);
        }


        public void ZetId(int nr)
        {
            if (nr < 0) throw new GebruikerException("ZetId - <= 0");
            Id = nr;
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
            Locatie = locatie ?? throw new GebruikerException("ZetLocatie - null");
        }

        public void VoegReservatieToe(Reservatie reservatie)
        {
            if (reservatie == null) throw new GebruikerException("VoegReservatieToe - null");
            if (_reservaties.Contains(reservatie)) throw new GebruikerException("VoegReservatieToe - reservatie bestaat al");
            _reservaties.Add(reservatie);
            if ((reservatie.Gebruiker != this) || (reservatie.Gebruiker == null)) reservatie.ZetGebruiker(this);
        }

        public void VerwijderReservatie(Reservatie reservatie)
        {
            if (reservatie == null) throw new GebruikerException("VerwijderReservatie - null");
            if (!_reservaties.Contains(reservatie)) throw new RestaurantException("VerwijderReservatie - reservatie bestaat niet");
            _reservaties.Remove(reservatie);
            reservatie.VerwijderGebruiker();
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

        public IReadOnlyList<Reservatie> GeefReservaties()
        {
            return _reservaties.AsReadOnly();
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

        public override bool Equals(object? obj)
        {
            return obj is Gebruiker gebruiker &&
                   Id == gebruiker.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}