namespace ReservatieServiceBeheerderRESTService.Model.Input
{
    public class LocatieRESTinputDTO
    {
        public int Postcode { get; set; }
        
        public string Gemeente { get; set; } = null!;
        
        public string? Straat { get; set; }
        
        public string? Huisnummer { get; set; }
        
    }
}
