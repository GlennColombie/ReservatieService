using ReservatieServiceBL.Entities;

namespace ReservatieServiceBeheerderRESTService.Model.Input
{
    public class RestaurantRESTinputUpdateDTO
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Email { get; set; }
        public string Telefoonnummer { get; set; }
        public Keuken Keuken { get; set; }
        public LocatieRESTinputDTO Locatie { get; set; }
    }
}
