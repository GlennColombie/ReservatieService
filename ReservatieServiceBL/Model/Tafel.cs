using ReservatieServiceBL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservatieServiceBL.Model
{
    public class Tafel
    {
        public Tafel(int tafelnummer, int aantalPlaatsen, bool isBezet, int restaurantId)
        {
            ZetTafelnummer(tafelnummer);
            ZetAantalPlaatsen(aantalPlaatsen);
            ZetIsBezet(isBezet);
            ZetRestaurantId(restaurantId);
        }
        public int Tafelnummer { get; private set; }
        public int AantalPlaatsen { get; private set; }
        public bool IsBezet { get; private set; }

        public int RestaurantId { get; private set; }

        public void ZetTafelnummer(int tafelnummer)
        {
            if (tafelnummer < 0) throw new TafelException("ZetTafelnummer: Tafelnummer moet groter zijn dan 0");
            Tafelnummer = tafelnummer;
        }

        public void ZetAantalPlaatsen(int aantal)
        {
            if (aantal < 0) throw new TafelException("ZetAantalPlaatsen: AantalPlaatsen moet groter zijn dan 0");
            AantalPlaatsen = aantal;
        }
        
        public void ZetIsBezet(bool isBezet)
        {
            IsBezet = isBezet;
        }

        public void ZetBezet()
        {
            IsBezet = true;
        }

        public void ZetVrij()
        {
            IsBezet = false;
        }

        public void ZetRestaurantId(int restaurantId)
        {
            if (restaurantId < 0) throw new TafelException("ZetRestaurantId: RestaurantId moet groter zijn dan 0");
            RestaurantId = restaurantId;
        }

        //public void VerwijderRestaurant()
        //{
        //    if (Restaurant != null && Restaurant.GeefTafels().Contains(this)) Restaurant.VerwijderTafel(this);
        //    Restaurant = null;
        //}
        public override bool Equals(object? obj)
        {
            return obj is Tafel tafel &&
                   Tafelnummer == tafel.Tafelnummer;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Tafelnummer);
        }
    }
}
