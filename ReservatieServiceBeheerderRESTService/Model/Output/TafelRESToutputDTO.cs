namespace ReservatieServiceBeheerderRESTService.Model.Output
{
    public class TafelRESToutputDTO
    {
        public TafelRESToutputDTO(int tafelnummer, int aantalPlaatsen)
        {
            Tafelnummer = tafelnummer;
            AantalPlaatsen = aantalPlaatsen;
        }

        public int Tafelnummer { get; set; }
        public int AantalPlaatsen { get; set; }
    }
}
