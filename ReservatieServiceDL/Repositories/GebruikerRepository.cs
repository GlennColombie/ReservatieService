using System.Data.SqlClient;
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
                cmd.CommandText = $"INSERT INTO Gebruiker (Id, Naam, Email, Telefoonnummer, LocatieId) VALUES ('{gebruiker.Id}', '{gebruiker.Naam}', '{gebruiker.Email}', '{gebruiker.TelefoonNr}', '{gebruiker.Locatie.Id}')";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new GebruikerRepositoryException("Kan geen verbinding maken met de database", ex);
            }
            finally
            {
                _connection.Close();
            }
        }
    }

    public void UpdateGebruiker(Gebruiker gebruiker)
    {
        throw new NotImplementedException();
    }

    public void VerwijderGebruiker(Gebruiker gebruiker)
    {
        throw new NotImplementedException();
    }

    public bool BestaatGebruiker(Gebruiker gebruiker)
    {
        using (SqlCommand cmd = _connection.CreateCommand())
        {
            try
            {
                _connection.Open();
                cmd.CommandText = $"SELECT count(*) FROM Gebruiker WHERE Naam = '{gebruiker.Naam}' AND Email = '{gebruiker.Email}' AND Telefoonnummer = '{gebruiker.TelefoonNr}'";
                int n = (int)cmd.ExecuteScalar();
                return n > 0;
            }
            catch (Exception ex)
            {
                throw new GebruikerRepositoryException("Kan geen verbinding maken met de database", ex);
            }
            finally
            {
                _connection.Close();
            }
        }
    }

    public bool BestaatGebruiker(int gebruiker)
    {
        throw new NotImplementedException();
    }

    public Gebruiker GeefGebruiker(int gebruikerKlantnr)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}