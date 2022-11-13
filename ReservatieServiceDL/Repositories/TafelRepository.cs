using Microsoft.Data.SqlClient;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservatieServiceDL.Repositories
{
    public class TafelRepository : ITafelRepository
    {
        private readonly SqlConnection _connection;
        public TafelRepository(string connectionString)
        {
            _connection = new(connectionString);
        }
        public void AddTafel(Tafel tafel, Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public void DeleteTafel(Tafel tafel, Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public Tafel GeefTafel(int tafelnummer, Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public void UpdateTafel(Tafel tafel, Restaurant restaurant)
        {
            throw new NotImplementedException();
        }
    }
}
