namespace ReservatieServiceGebruikerRESTService.Model.Input
{
    public class ReservatieRESTinputUpdateDTO
    {
        public DateTime Datum { get; set; }
        public DateTime Uur { get; set; }
        public int AantalPlaatsen { get; set; }
    }
}
