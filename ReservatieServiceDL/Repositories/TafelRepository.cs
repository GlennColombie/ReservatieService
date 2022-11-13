using Microsoft.Data.SqlClient;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;
using ReservatieServiceDL.Exceptions;
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
        public void VoegTafelToe(Tafel tafel, Restaurant restaurant)
        {
            if (tafel == null) throw new TafelRepositoryException("VoegTafelToe: Tafel mag niet null zijn");
            if (restaurant == null) throw new TafelRepositoryException("VoegTafelToe: Restaurant mag niet null zijn");
            using SqlCommand cmd = _connection.CreateCommand();
            try
            {
                _connection.Open();
                cmd.CommandText = $"insert into tafel (tafelnummer, aantalplaatsen, isbezet, restaurantid, is_visible) values ({tafel.Tafelnummer}, {tafel.AantalPlaatsen}, '{tafel.IsBezet}', {restaurant.Id}, 1)";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new TafelRepositoryException("VoegTafelToe: " + ex.Message);
            }
        }

        public void VerwijderTafel(Tafel tafel, Restaurant restaurant)
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

        public bool BestaatTafel(int tafelnummer, Restaurant restaurant)
        {
            if (tafelnummer < 0) throw new TafelRepositoryException("BestaatTafel: Tafelnummer moet groter zijn dan 0");
            if (restaurant == null) throw new TafelRepositoryException("BestaatTafel: restaurant null");
            using SqlCommand cmd = _connection.CreateCommand();
            try
            {
                _connection.Open();
                cmd.CommandText = $"select count(*) from tafel where tafelnummer = {tafelnummer} and restaurantid = {restaurant.Id}";
                int n = (int)cmd.ExecuteScalar();
                return n > 0;
            }
            catch (Exception e)
            {
                throw new TafelRepositoryException("BestaatTafel: " + e.Message);
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
