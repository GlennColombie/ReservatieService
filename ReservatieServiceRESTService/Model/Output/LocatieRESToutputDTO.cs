namespace ReservatieServiceGebruikerRESTService.Model.Output
{
    public class LocatieRESToutputDTO
    {
        public LocatieRESToutputDTO(int postcode, string gemeente)
        {
            Postcode = postcode;
            Gemeente = gemeente;
        }

        public LocatieRESToutputDTO(int postcode, string gemeente, string? straat) : this(postcode, gemeente)
        {
            Straat = straat;
        }

        public LocatieRESToutputDTO(int postcode, string gemeente, string? straat, string? huisnummer)
        {
            Postcode = postcode;
            Gemeente = gemeente;
            Straat = straat;
            Huisnummer = huisnummer;
        }

        public int Postcode { get; private set; }

        public string Gemeente { get; private set; } = null!;

        public string? Straat { get; private set; }

        public string? Huisnummer { get; private set; }

    }
}
