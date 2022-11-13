using ReservatieServiceBL.Exceptions;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ReservatieServiceBL.Managers
{
    public class TafelManager
    {
        private ITafelRepository _tafelRepository;
        private IRestaurantRepository _restaurantRepository;

        public TafelManager(ITafelRepository tafelRepository, IRestaurantRepository restaurantRepository)
        {
            _tafelRepository = tafelRepository;
            _restaurantRepository = restaurantRepository;
        }

        

        

        //public Tafel GeefTafel(int tafelnummer)
        //{
        //    if (tafelnummer < 0) throw new TafelManagerException("GetTafel: Tafelnummer moet groter zijn dan 0");
        //    try
        //    {
        //        return _tafelRepository.GeefTafel(tafelnummer);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new TafelManagerException("DeleteTafel: Tafel mag niet null zijn");
        //    }
        //}

        //public List<Tafel> GetTafels()
        //{
        //    try
        //    {
        //        return _tafelRepository.GetTafels();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new TafelManagerException("DeleteTafel: Tafel mag niet null zijn");
        //    }
        //}
    }
}
