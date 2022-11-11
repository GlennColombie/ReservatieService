using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;
using ReservatieServiceDL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservatieServiceDL.Repositories
{
    public class LocatieRepository : ILocatieRepository
    {
        private readonly SqlConnection _connection;
        public LocatieRepository(string connectionString)
        {
            _connection = new(connectionString);
        }
        public bool BestaatLocatie(Locatie locatie)
        {
            if (locatie == null) throw new LocatieRepositoryException("BestaatLocatie - null");
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"SELECT count(*) from locatie where postcode = {locatie.Postcode} AND gemeente = '{locatie.Gemeente}' and straat = '{locatie.Straat}' and huisnummer = '{locatie.Huisnummer}'";
                    int n = (int)cmd.ExecuteScalar();
                    return n > 0;
                }
                catch (Exception ex)
                {
                    throw new LocatieRepositoryException("BestaatLocatie - repo", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public void UpdateLocatie(Locatie locatie)
        {
            if (locatie == null) throw new LocatieRepositoryException("BestaatLocatie - null");
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"UPDATE locatie SET postcode = {locatie.Postcode}, gemeente = '{locatie.Gemeente}', straat = '{locatie.Straat}', huisnummer = '{locatie.Huisnummer}' WHERE id = {locatie.Id}";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new LocatieRepositoryException("BestaatLocatie - repo", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public void VerwijderLocatie(Locatie locatie)
        {
            if (locatie == null) throw new LocatieRepositoryException("BestaatLocatie - null");
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"UPDATE locatie SET is_visible = 0 WHERE id = {locatie.Id}";
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new LocatieRepositoryException("BestaatLocatie - repo", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public void VoegLocatieToe(Locatie locatie)
        {
            if (locatie == null) throw new LocatieRepositoryException("BestaatLocatie - null");
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"insert into locatie (postcode, gemeente, straat, huisnummer, is_visible) output inserted.id values ({locatie.Postcode}, '{locatie.Gemeente}', '{locatie.Straat}', '{locatie.Huisnummer}', 1)";
                    int id = (int)cmd.ExecuteScalar();
                    locatie.ZetId(id);
                }
                catch (Exception ex)
                {
                    throw new LocatieRepositoryException("BestaatLocatie - repo", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        //public Locatie GeefLocatie(int id)
        //{
        //    using (SqlCommand cmd = _connection.CreateCommand())
        //    {
        //        try
        //        {
        //            _connection.Open();
        //            cmd.CommandText = $"SELECT * from locatie where id = {id}";
        //            SqlDataReader reader = cmd.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                int postcode = (int)reader["postcode"];
        //                string gemeente = (string)reader["gemeente"];
        //                string straat = (string)reader["straat"];
        //                string huisnummer = (string)reader["huisnummer"];
        //                return new Locatie(id, postcode, gemeente, straat, huisnummer);
        //            }
        //            return null;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new LocatieRepositoryException("BestaatLocatie - repo", ex);
        //        }
        //        finally
        //        {
        //            _connection.Close();
        //        }
        //    }
        //}
    }
}
