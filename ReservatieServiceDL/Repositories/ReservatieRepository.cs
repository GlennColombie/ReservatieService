using System.Data.SqlClient;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservatieServiceDL.Exceptions;

namespace ReservatieServiceDL.Repositories
{
    public class ReservatieRepository : IReservatieRepository
    {
        private SqlConnection _connection;

        public ReservatieRepository(string connectionString)
        {
            _connection = new(connectionString);
        }
        public bool BestaatReservatie(Reservatie reservatie)
        {
            if (reservatie == null) throw new ReservatieRepositoryException("BestaatReservatie - null");
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"SELECT count(*) from reservatie where restaurantid = {reservatie.Restaurant.Id} and datum = '{reservatie.Datum.ToShortDateString()}' AND uur = '{reservatie.Uur.ToShortTimeString()}' and einduur = '{reservatie.Einduur.ToShortTimeString()}' and GebruikerId = {reservatie.Gebruiker.Id}" +
                        $" and tafelnummer = {reservatie.Tafel.Tafelnummer}";
                    int n = (int)cmd.ExecuteScalar();
                    return n > 0;
                }
                catch (Exception ex)
                {
                    throw new ReservatieRepositoryException("BestaatReservatie - repo", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public void DeleteReservatie(Reservatie reservatie)
        {
            throw new NotImplementedException();
        }

        public Reservatie GeefReservatie(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateReservatie(Reservatie reservatie)
        {
            throw new NotImplementedException();
        }

        public void VoegReservatieToe(Reservatie reservatie)
        {
            if (reservatie == null) throw new ReservatieRepositoryException("VoegReservatieToe - null");
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"INSERT INTO reservatie (restaurantid, datum, uur, einduur, GebruikerId, tafelnummer) output inserted.reservatienummer VALUES ({reservatie.Restaurant.Id}, '{reservatie.Datum}', '{reservatie.Uur}', '{reservatie.Einduur}', {reservatie.Gebruiker.Id}, {reservatie.Tafel.Tafelnummer})";
                    int id = (int)cmd.ExecuteScalar();
                    reservatie.ZetReservatienummer(id);
                }
                catch (Exception ex)
                {
                    throw new ReservatieRepositoryException("VoegReservatieToe - repo", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public IEnumerable<Reservatie> ZoekReservaties(DateTime? begindatum, DateTime? einddatum)
        {
            throw new NotImplementedException();
        }
    }
}
