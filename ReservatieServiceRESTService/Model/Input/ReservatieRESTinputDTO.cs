﻿using ReservatieServiceBL.Entities;
using ReservatieServiceGebruikerRESTService.Model.Output;

namespace ReservatieServiceGebruikerRESTService.Model.Input
{
    public class ReservatieRESTinputDTO
    {
        public DateTime Datum { get; set; }

        public DateTime Uur { get; set; }

        public int AantalPlaatsen { get; set; }

        public TafelRESTinputDTO Tafel { get; set; } = null!;
    }
}
