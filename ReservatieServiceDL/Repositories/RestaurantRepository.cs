using System.Data.SqlClient;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;
using ReservatieServiceDL.Exceptions;
using System.Data.SqlClient;

namespace ReservatieServiceDL.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly SqlConnection _connection;
        public RestaurantRepository(string connectionString)
        {
            _connection = new(connectionString);
        }
        public bool BestaatRestaurant(Restaurant restaurant)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"select count(*) from Restaurant where naam = '{restaurant.Naam}' and locatieid = {restaurant.Locatie.Id} and telefoonnummer = '{restaurant.Telefoonnummer}' and email = '{restaurant.Email}' and keuken = '{restaurant.Keuken}'";
                    int n = (int)cmd.ExecuteScalar();
                    return n > 0;
                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException("GebruikerRegistreren - repo", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public bool BestaatRestaurant(int id)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"select count(*) from Restaurant where id = {id}";
                    int n = (int)cmd.ExecuteScalar();
                    return n > 0;
                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException("GebruikerRegistreren - repo", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public IReadOnlyList<Restaurant> GeefAlleBestaandeRestaurants()
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
                    Keuken keuken;
                    Locatie l = null;
                    Restaurant r = null;
                    List<Restaurant> restaurants = new();
                    cmd.CommandText = $"select r.Id RestaurantId, r.naam, r.email, r.telefoonnummer, l.id locatieid, l.postcode, l.gemeente, l.straat, l.huisnummer " +
                        $"from Restaurant r " +
                        $"left join locatie l on r.locatieid = l.id " +
                        $"where is_visible = 1";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        naam = (string)reader["naam"];
                        email = (string)reader["Email"];
                        telefoonnummer = (string)reader["telefoonnummer"];
                        postcode = (int)reader["Postcode"];
                        gemeente = (string)reader["Gemeente"];
                        straat = reader["Straat"] == DBNull.Value ? null : (string)reader["Straat"];
                        huisnummer = reader["Huisnummer"] == DBNull.Value ? null : (string)reader["Huisnummer"];
                        keuken = (Enum.Parse<Keuken>((string)reader["keuken"]));

                        l = new(postcode, gemeente, straat, huisnummer);
                        l.ZetId((int)reader["LocatieId"]);

                        r = new(naam, l, telefoonnummer, email, keuken);
                        r.ZetId((int)reader["RestaurantId"]);
                        restaurants.Add(r);
                    }
                    reader.Close();
                    return restaurants;
                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException("GeefAlleRestaurants - repo", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public IReadOnlyList<Restaurant> GeefAlleRestaurants()
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
                    Keuken keuken;
                    Locatie l = null;
                    Restaurant r = null;
                    List<Restaurant> restaurants = new();
                    cmd.CommandText = $"select r.Id RestaurantId, r.naam, r.email, r.telefoonnummer, l.id locatieid, l.postcode, l.gemeente, l.straat, l.huisnummer " +
                        $"from Restaurant r " +
                        $"left join locatie l on r.locatieid = l.id";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        naam = (string)reader["naam"];
                        email = (string)reader["Email"];
                        telefoonnummer = (string)reader["telefoonnummer"];
                        postcode = (int)reader["Postcode"];
                        gemeente = (string)reader["Gemeente"];
                        straat = reader["Straat"] == DBNull.Value ? null : (string)reader["Straat"];
                        huisnummer = reader["Huisnummer"] == DBNull.Value ? null : (string)reader["Huisnummer"];
                        keuken = (Enum.Parse<Keuken>((string)reader["keuken"]));

                        l = new(postcode, gemeente, straat, huisnummer);
                        l.ZetId((int)reader["LocatieId"]);

                        r = new(naam, l, telefoonnummer, email, keuken);
                        r.ZetId((int)reader["RestaurantId"]);
                        restaurants.Add(r);
                    }
                    reader.Close();
                    return restaurants;
                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException("GeefAlleRestaurants - repo", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }

        public Restaurant GeefRestaurant(int id)
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
                    Keuken keuken;
                    Locatie l = null;
                    Restaurant r = null;
                    cmd.CommandText = $"select r.Id RestaurantId, r.naam, r.email, r.telefoonnummer, l.id locatieid, l.postcode, l.gemeente, l.straat, l.huisnummer " +
                        $"from Restaurant r " +
                        $"left join locatie l on r.locatieid = l.id " +
                        $"where id = {id}";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        naam = (string)reader["naam"];
                        email = (string)reader["Email"];
                        telefoonnummer = (string)reader["telefoonnummer"];
                        postcode = (int)reader["Postcode"];
                        gemeente = (string)reader["Gemeente"];
                        straat = reader["Straat"] == DBNull.Value ? null : (string)reader["Straat"];
                        huisnummer = reader["Huisnummer"] == DBNull.Value ? null : (string)reader["Huisnummer"];
                        keuken = (Enum.Parse<Keuken>((string)reader["keuken"]));

                        l = new(postcode, gemeente, straat, huisnummer);
                        l.ZetId((int)reader["LocatieId"]);

                        r = new(naam, l, telefoonnummer, email, keuken);
                        r.ZetId((int)reader["RestaurantId"]);
                    }
                    reader.Close();
                    return r;
                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException("GebruikerRegistreren - repo", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }


        public IReadOnlyList<Restaurant> GeefRestaurantsVanLocatie(Locatie locatie)
        {
            throw new NotImplementedException();
        }

        public void UpdateRestaurant(Restaurant restaurant)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"update restaurant naam = {restaurant.Naam}, email = {restaurant.Email}, telefoonnummer = {restaurant.Telefoonnummer}, keuken = {restaurant.Keuken} where id = {restaurant.Id}";
                    cmd.ExecuteNonQuery();
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

        public void VerwijderRestaurant(Restaurant restaurant)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"update restaurant is_visible = 0";
                    cmd.ExecuteNonQuery();
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

        public void VoegRestaurantToe(Restaurant restaurant)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"insert into restaurant (naam, email, telefoonnummer, locatieid, keuken, is_visible) " +
                        $" output inserted.id values ('{restaurant.Naam}', '{restaurant.Email}', '{restaurant.Telefoonnummer}', {restaurant.Locatie.Id}, '{restaurant.Keuken}', 1)";
                    int id = (int)cmd.ExecuteScalar();
                    restaurant.ZetId(id);
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
}
