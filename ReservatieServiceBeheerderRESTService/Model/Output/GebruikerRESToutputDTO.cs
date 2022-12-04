namespace ReservatieServiceBeheerderRESTService.Model.Output
{
    public class GebruikerRESToutputDTO
    {
        public GebruikerRESToutputDTO(int gebruikerId, string naam, string email, string telefoonnummer, LocatieRESToutputDTO locatie)
        {
            GebruikerId = gebruikerId;
            Naam = naam;
            Email = email;
            Telefoonnummer = telefoonnummer;
            Locatie = locatie;
            //Reservaties = reservaties;
        }

        public int GebruikerId { get; set; }
        public string Naam { get; set; }

        public string Email { get; set; }

        public string Telefoonnummer { get; set; }

        public LocatieRESToutputDTO Locatie { get; set; }

        //public List<ReservatieRESToutputDTO> Reservaties { get; set; }
    }
}
