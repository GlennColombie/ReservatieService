namespace ReservatieServiceGebruikerRESTService.Model.Output
{
    public class RestaurantRESToutputDTO
    {
        public RestaurantRESToutputDTO()
        {
        }

        public RestaurantRESToutputDTO(int id, string naam, string email, string telefoonnummer, LocatieRESToutputDTO locatie, List<TafelRESToutputDTO> tafels)
        {
            Id = id;
            Naam = naam;
            Email = email;
            Telefoonnummer = telefoonnummer;
            Locatie = locatie;
            Tafels = tafels;
        }
        public int Id { get; set; }

        public string Naam { get; set; }

        public string Email { get; set; }

        public string Telefoonnummer { get; set; }

        public LocatieRESToutputDTO Locatie { get; set; }

        public List<TafelRESToutputDTO> Tafels { get; set; }
    }
}
