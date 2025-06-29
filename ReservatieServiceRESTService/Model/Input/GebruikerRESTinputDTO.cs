﻿namespace ReservatieServiceGebruikerRESTService.Model.Input
{
    public class GebruikerRESTinputDTO
    {
        public string Naam { get; set; } = null!;
              
        public string Email { get; set; } = null!;
        
        public string Telefoonnummer { get; set; } = null!;

        public LocatieRESTinputDTO Locatie { get; set; } = null!;
    }
}
