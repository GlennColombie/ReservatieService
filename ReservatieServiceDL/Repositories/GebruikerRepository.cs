using System.Data.SqlClient;
using System.IO;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;
using ReservatieServiceDL.Exceptions;

namespace ReservatieServiceDL;

public class GebruikerRepository : IGebruikerRepository
{
    private readonly SqlConnection _connection;

    public GebruikerRepository(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
    }

    public void GebruikerRegistreren(Gebruiker gebruiker)
    {
        using (SqlCommand cmd = _connection.CreateCommand())
        {
            try
            {
                _connection.Open();
                cmd.CommandText = $"INSERT INTO Gebruiker (Naam, Email, Telefoonnummer, LocatieId, Is_visible) output INSERTED.Id VALUES ('{gebruiker.Naam}', '{gebruiker.Email}', '{gebruiker.Telefoonnummer}', '{gebruiker.Locatie.Id}', 1)";
                int id = (int)cmd.ExecuteScalar();
                gebruiker.ZetId(id);
            }
            catch (Exception ex)
            {
                throw new GebruikerRepositoryException("GebruikerRegistreren - repo", ex);
            }
            finally
            {
                _connection.Close();
            }
        }
    }

    public void UpdateGebruiker(Gebruiker gebruiker)
    {
        using (SqlCommand cmd = _connection.CreateCommand())
        {
            try
            {
                _connection.Open();
                cmd.CommandText = $"UPDATE Gebruiker SET Naam = '{gebruiker.Naam}', Email = '{gebruiker.Email}', Telefoonnummer = '{gebruiker.Telefoonnummer}', LocatieId = {gebruiker.Locatie.Id} " +
                    $"WHERE Id = {gebruiker.Id}";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new GebruikerRepositoryException("UpdateGebruiker - repo", ex);
            }
            finally
            {
                _connection.Close();
            }
        }
    }

    public void VerwijderGebruiker(Gebruiker gebruiker)
    {
        using (SqlCommand cmd = _connection.CreateCommand())
        {
            try
            {
                _connection.Open();
                cmd.CommandText = $"UPDATE Gebruiker SET Is_visible = 0 " +
                    $" WHERE Id = '{gebruiker.Id}'";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new GebruikerRepositoryException("VerwijderGebruiker - repo", ex);
            }
            finally
            {
                _connection.Close();
            }
        }
    }

    public bool BestaatGebruiker(Gebruiker gebruiker)
    {
        using (SqlCommand cmd = _connection.CreateCommand())
        {
            try
            {
                _connection.Open();
                cmd.CommandText = $"SELECT count(*) FROM Gebruiker WHERE Naam = '{gebruiker.Naam}' AND Email = '{gebruiker.Email}' AND Telefoonnummer = '{gebruiker.Telefoonnummer}' AND LocatieId = {gebruiker.Locatie.Id}";
                int n = (int)cmd.ExecuteScalar();
                return n > 0;
            }
            catch (Exception ex)
            {
                throw new GebruikerRepositoryException("BestaatGebruiker - repo", ex);
            }
            finally
            {
                _connection.Close();
            }
        }
    }

    public bool BestaatGebruiker(int id)
    {
        using (SqlCommand cmd = _connection.CreateCommand())
        {
            try
            {
                _connection.Open();
                cmd.CommandText = $"SELECT count(*) FROM Gebruiker WHERE Id = {id}";
                int n = (int)cmd.ExecuteScalar();
                return n > 0;
            }
            catch (Exception ex)
            {
                throw new GebruikerRepositoryException("BestaatGebruiker - repo", ex);
            }
            finally
            {
                _connection.Close();
            }
        }
    }

    public Gebruiker GeefGebruiker(int id)
    {
        using (SqlCommand cmd = _connection.CreateCommand())
        {
            try
            {
                _connection.Open();
                string naam;
                string email;
                string telefoonnummer;
                int postcode;
                string gemeente;
                string straat;
                string huisnummer;
                Locatie l = null;
                Gebruiker g = null;
                cmd.CommandText = $"SELECT g.Id, g.Naam, g.Email, g.Telefoonnummer, l.Id LocatieId, l.Postcode, l.Gemeente, l.straat, l.huisnummer" +
                    $" FROM Gebruiker g" +
                    $" left JOIN Locatie l" +
                    $" ON g.LocatieId = l.Id" +
                    $" WHERE g.Id = {id}";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    naam = (string)reader["Naam"];
                    email = (string)reader["Email"];
                    telefoonnummer = (string)reader["Telefoonnummer"];
                    postcode = (int)reader["Postcode"];
                    gemeente = (string)reader["Gemeente"];
                    straat = reader["Straat"] == DBNull.Value ? null : (string)reader["Straat"];
                    huisnummer = reader["Huisnummer"] == DBNull.Value ? null : (string)reader["Huisnummer"];
                    
                    l = new(postcode, gemeente, straat, huisnummer);
                    l.ZetId((int)reader["LocatieId"]);
                    
                    g = new(naam, email, telefoonnummer, l);
                    g.ZetId((int)reader["Id"]);
                }
                reader.Close();
                return g;
            }
            catch (Exception ex)
            {
                throw new GebruikerRepositoryException("GeefGebruiker - repo", ex);
            }
            finally
            {
                _connection.Close();
            }
        }
    }

    public IReadOnlyList<Reservatie> ZoekReservaties(DateTime? begindatum, DateTime? einddatum)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Reservatie> GeefReservaties(DateTime? begindatum, DateTime? einddatum)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Gebruiker> GeefGebruikers()
    {
        using (SqlCommand cmd = _connection.CreateCommand())
        {
            try
            {
                List<Gebruiker> gebruikers = new();
                string naam;
                string email;
                string telefoonnummer;
                int postcode;
                string gemeente;
                string straat;
                string huisnummer;
                Locatie l = null;
                Gebruiker g = null;
                _connection.Open();
                cmd.CommandText = $"SELECT g.Id GebruikerId, g.Naam, g.Email, g.Telefoonnummer, l.Id LocatieId, l.Gemeente, l.Postcode, l.Straat, l.Huisnummer FROM Gebruiker g left join Locatie l on g.LocatieId = l.Id";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    naam = (string)reader["Naam"];
                    email = (string)reader["Email"];
                    telefoonnummer = (string)reader["Telefoonnummer"];
                    postcode = (int)reader["Postcode"];
                    gemeente = (string)reader["Gemeente"];
                    straat = reader["Straat"] == DBNull.Value ? null : (string)reader["Straat"];
                    huisnummer = reader["Huisnummer"] == DBNull.Value ? null : (string)reader["Huisnummer"];

                    l = new(postcode, gemeente, straat, huisnummer);
                    l.ZetId((int)reader["LocatieId"]);

                    g = new(naam, email, telefoonnummer, l);
                    g.ZetId((int)reader["GebruikerId"]);
                    gebruikers.Add(g);
                }
                reader.Close();
                return gebruikers;
            }
            catch (Exception ex)
            {
                throw new GebruikerRepositoryException("GeefGebruikers - repo", ex);
            }
            finally
            {
                _connection.Close();
            }
        }
    }

    public IReadOnlyList<Gebruiker> GeefBestaandeGebruikers()
    {
        using (SqlCommand cmd = _connection.CreateCommand())
        {
            try
            {
                List<Gebruiker> gebruikers = new();
                string naam;
                string email;
                string telefoonnummer;
                int postcode;
                string gemeente;
                string straat;
                string huisnummer;
                Locatie l = null;
                Gebruiker g = null;
                _connection.Open();
                cmd.CommandText = $"SELECT g.Id GebruikerId, g.Naam, g.Email, g.Telefoonnummer, l.Id LocatieId, l.Gemeente, l.Postcode, l.Straat, l.Huisnummer FROM Gebruiker g left join Locatie l on g.LocatieId = l.Id where is_visible = 1";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    naam = (string)reader["Naam"];
                    email = (string)reader["Email"];
                    telefoonnummer = (string)reader["Telefoonnummer"];
                    postcode = (int)reader["Postcode"];
                    gemeente = (string)reader["Gemeente"];
                    straat = reader["Straat"] == DBNull.Value ? null : (string)reader["Straat"];
                    huisnummer = reader["Huisnummer"] == DBNull.Value ? null : (string)reader["Huisnummer"];

                    l = new(postcode, gemeente, straat, huisnummer);
                    l.ZetId((int)reader["LocatieId"]);

                    g = new(naam, email, telefoonnummer, l);
                    g.ZetId((int)reader["GebruikerId"]);
                    gebruikers.Add(g);
                }
                reader.Close();
                return gebruikers;
            }
            catch (Exception ex)
            {
                throw new GebruikerRepositoryException("GeefGebruikers - repo", ex);
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}