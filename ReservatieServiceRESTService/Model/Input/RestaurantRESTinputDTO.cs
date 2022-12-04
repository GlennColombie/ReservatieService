namespace ReservatieServiceGebruikerRESTService.Model.Input
{
    public class RestaurantRESTinputDTO
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Email { get; set; }
        public string Telefoonnummer { get; set; }
        public LocatieRESTinputDTO Locatie { get; set; }
    }
}
