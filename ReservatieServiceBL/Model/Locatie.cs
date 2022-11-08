using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservatieServiceBL.Exceptions;

namespace ReservatieServiceBL.Model
{
    public class Locatie
    {
        public Locatie(int id, int postcode, string gemeente, string straat, string huisnummer)
        {
            ZetId(id);
            ZetPostcode(postcode);
            ZetGemeente(gemeente);
            ZetStraat(straat);
            ZetHuisnummer(huisnummer);
            
        }
        public int Id { get; private set; }
        public int Postcode { get; private set; }
        public string Gemeente { get; private set; }
        public string Straat { get; private set; }
        public string Huisnummer { get; private set; }

        public void ZetId(int id)
        {
            if (id < 0) throw new LocatieException("Id mag niet kleiner zijn dan 0");
            Id = id;
        }
        
        public void ZetPostcode(int postcode)
        {
            if (postcode is < 0 or > 9999) throw new LocatieException("Postcode mag niet kleiner zijn dan 0 of groter dan 9999");
            Postcode = postcode;
        }
        
        public void ZetGemeente(string gemeente)
        {
            if (string.IsNullOrWhiteSpace(gemeente)) throw new LocatieException("Gemeente mag niet leeg zijn");
            Gemeente = gemeente;
        }
        
        public void ZetStraat(string straat)
        {
            if (string.IsNullOrWhiteSpace(straat)) throw new LocatieException("Straat mag niet leeg zijn");
            Straat = straat;
        }
        
        public void ZetHuisnummer(string huisnummer)
        {
            if (string.IsNullOrWhiteSpace(huisnummer)) throw new LocatieException("Huisnummer mag niet leeg zijn");
            Huisnummer = huisnummer;
        }

        public bool IsDezelfde(Locatie locatie)
        {
            if (locatie == null) throw new LocatieException("Locatie is null");
            if (locatie.Id != Id) return false;
            if (locatie.Postcode != Postcode) return false;
            if (!locatie.Postcode.Equals(Postcode)) return false;
            if (!locatie.Gemeente.Equals(Gemeente)) return false;
            return true;
        }
    }
}