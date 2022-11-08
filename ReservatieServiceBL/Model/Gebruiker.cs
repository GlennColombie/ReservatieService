using ReservatieServiceBL.Exceptions;

namespace ReservatieServiceBL.Model
{
    public class Gebruiker
    {
        public int Id { get; private set; }
        public string Naam { get; private set; }
        public string Email { get; private set; }
        public string TelefoonNr { get; private set; }
        public Locatie Locatie { get; private set; }

        private List<Reservatie> _reservaties = new();

        public Gebruiker(int id, string naam, string email, string nr, Locatie locatie)
        {
            ZetId(id);
            ZetNaam(naam);
            ZetEmail(email);
            ZetTelefoonnr(nr);
            ZetLocatie(locatie);
        }
        
        public void ZetId(int nr)
        {
            if (nr < 0) throw new GebruikerException("ZetKlantnr - <0");
            Id = nr;
        }
        public void ZetNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam)) throw new GebruikerException("ZetNaam - null/whitespace");
            Naam = naam;
        }
        public void ZetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new GebruikerException("ZetNaam - null/whitespace");
            Email = email;
        }
        public void ZetTelefoonnr(string nr)
        {
            if (string.IsNullOrWhiteSpace(nr)) throw new GebruikerException("ZetNaam - null/whitespace");
            TelefoonNr = nr;
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
            if (!TelefoonNr.Equals(gebruiker.TelefoonNr)) return false;
            if (!Locatie.IsDezelfde(gebruiker.Locatie)) return false;
            return true;
        }

        public IReadOnlyList<Reservatie> GeefReservaties()
        {
            return _reservaties.AsReadOnly();
        }
    }
}