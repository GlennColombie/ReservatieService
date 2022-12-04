using ReservatieServiceBL.Entities;

namespace ReservatieServiceBeheerderRESTService.Model.Output
{
    public class RestaurantRESToutputDTO
    {
        public RestaurantRESToutputDTO()
        {
        }

        public RestaurantRESToutputDTO(int id, string naam, string email, string telefoonnummer, Keuken keuken, LocatieRESToutputDTO locatie, List<TafelRESToutputDTO> tafels, List<ReservatieRESToutputDTO> reservaties)
        {
            Id = id;
            Naam = naam;
            Email = email;
            Telefoonnummer = telefoonnummer;
            Keuken = keuken;
            Locatie = locatie;
            Tafels = tafels;
            Reservaties = reservaties;
        }
        public int Id { get; set; }

        public string Naam { get; set; }

        public string Email { get; set; }

        public string Telefoonnummer { get; set; }

        public Keuken Keuken { get; set; }

        public LocatieRESToutputDTO Locatie { get; set; }

        public List<TafelRESToutputDTO> Tafels { get; set; }

        public List<ReservatieRESToutputDTO> Reservaties { get; set; }
    }
}
