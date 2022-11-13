using ReservatieServiceBL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservatieServiceBL.Interfaces
{
    public interface ITafelRepository
    {
        void AddTafel(Tafel tafel, Restaurant restaurant);
        void DeleteTafel(Tafel tafel, Restaurant restaurant);
        void UpdateTafel(Tafel tafel, Restaurant restaurant);
        Tafel GeefTafel(int tafelnummer, Restaurant restaurant);
        //List<Tafel> GeefTafels();
    }
}
