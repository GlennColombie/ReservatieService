using ReservatieServiceBL.Entities;

namespace ReservatieServiceBeheerderRESTService.Model.Output
{
    public class ReservatieRESToutputDTO
    {
        public ReservatieRESToutputDTO()
        {
        }

        public ReservatieRESToutputDTO(int reservatienummer, int tafelnummer, DateTime datum, DateTime uur, DateTime einduur, int aantalPlaatsen, GebruikerRESToutputDTO gebruiker, TafelRESToutputDTO tafel)
        {
            Reservatienummer = reservatienummer;
            Tafelnummer = tafelnummer;
            Datum = datum.ToShortDateString();
            Uur = uur.ToShortTimeString();
            Einduur = einduur.ToShortTimeString();
            AantalPlaatsen = aantalPlaatsen;
            //Restaurant = restaurant;
            Gebruiker = gebruiker;
            Tafel = tafel;
        }

        public int Reservatienummer { get; set; } 

        public int Tafelnummer { get; set; } 

        public string Datum { get; set; } 

        public string Uur { get; set; }

        public string Einduur { get; set; }

        public int AantalPlaatsen { get; set; }

        public GebruikerRESToutputDTO Gebruiker { get; set; } = null!;

        //public RestaurantRESToutputDTO Restaurant { get; set; } = null!;

        public TafelRESToutputDTO Tafel { get; set; } = null!;
    }
}
