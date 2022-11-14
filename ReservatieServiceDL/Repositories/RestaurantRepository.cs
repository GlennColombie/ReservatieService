using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Model;
using ReservatieServiceDL.Exceptions;

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
                    cmd.CommandText = $"select r.Id RestaurantId, r.naam, r.email, r.telefoonnummer, r.keuken, l.id locatieid, l.postcode, l.gemeente, l.straat, l.huisnummer " +
                        $"from Restaurant r " +
                        $"left join locatie l on r.locatieid = l.id " +
                        $"where r.is_visible = 1";
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



        // WERKT
        public IReadOnlyList<Restaurant> GeefAlleRestaurants()
        {
            using SqlCommand cmd = _connection.CreateCommand();
            try
            {
                _connection.Open();
                List<Restaurant> restaurants = new();
                Dictionary<int, Locatie> locaties = new();
                int restaurantIdOld = -1;
                Restaurant r = null;
                Tafel t = null;
                cmd.CommandText = "select r.Id RestaurantId, r.naam, r.email, r.telefoonnummer, r.keuken, l.id locatieid, l.postcode, l.gemeente, l.straat, l.huisnummer, t.Tafelnummer, t.aantalplaatsen, t.isbezet " +
                    "from Restaurant r " +
                    "left join locatie l on r.locatieid = l.id " +
                    "left join tafel t on r.id = t.restaurantid " +
                    "order by r.id";
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int restaurantId = (int)reader["RestaurantId"];
                    if (restaurantId != restaurantIdOld)
                    {
                        Locatie l = null;
                        if (reader["LocatieId"] is int locId && !locaties.TryGetValue(locId, out l))
                        {
                            //TODO: get location fields
                            int postcode = (int)reader["Postcode"];
                            string gemeente = (string)reader["Gemeente"];
                            string straat = reader["Straat"] == DBNull.Value ? null : (string)reader["Straat"];
                            string huisnummer = reader["Huisnummer"] == DBNull.Value ? null : (string)reader["Huisnummer"];
                            l = new(locId, postcode, gemeente, straat, huisnummer);
                            locaties.Add(locId, l);
                        }

                        //TODO: get restaurant fields
                        Keuken keuken = (Enum.Parse<Keuken>((string)reader["keuken"]));
                        string naam = (string)reader["naam"];
                        string email = (string)reader["Email"];
                        string telefoonnummer = (string)reader["telefoonnummer"];
                        r = new(restaurantId, naam, l, telefoonnummer, email, keuken);
                        restaurants.Add(r);
                        restaurantIdOld = restaurantId;
                    }
                    if (reader["Tafelnummer"] is int tafelnummer)
                    {
                        //TODO: get table fields
                        // No need to test whether this is a new table. Tables are unique.
                        int aantalPlaatsen = (int)reader["AantalPlaatsen"];
                        bool isBezet = (bool)reader["IsBezet"];
                        t = new(tafelnummer, aantalPlaatsen, isBezet, restaurantId);
                        r.VoegTafelToe(t);
                    }
                }
                reader.Close();
                return restaurants;
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


        // ??
        public IReadOnlyList<Tafel> GeefAlleTafelsVanRestaurant(int id)
        {
            using SqlCommand cmd = _connection.CreateCommand();
            try
            {
                _connection.Open();
                int tafelnummer;
                int aantalPlaatsen;
                bool isBezet = true;
                cmd.CommandText = $"SELECT * FROM Tafel WHERE RestaurantId = {id} and is_visible = 1";
                SqlDataReader reader = cmd.ExecuteReader();
                List<Tafel> tafels = new();
                while (reader.Read())
                {
                    tafelnummer = (int)reader["Tafelnummer"];
                    aantalPlaatsen = (int)reader["AantalPlaatsen"];
                    isBezet = (bool)reader["IsBezet"];
                    Tafel t = new(tafelnummer, aantalPlaatsen, isBezet, id);
                    tafels.Add(t);
                }
                reader.Close();
                return tafels;
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

        // WERKT
        public Restaurant GeefRestaurant(int id)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    int tafelnummerOld = -1;
                    int tafelnummer = 0;
                    int aantalPlaatsen = 0;
                    bool isBezet = false;
                    bool first = true;
                    Locatie l = null;
                    Restaurant r = null;
                    List<Tafel> tafels = new();
                    cmd.CommandText = $"select r.Id RestaurantId, r.naam, r.email, r.telefoonnummer, r.keuken, l.id locatieid, l.postcode, l.gemeente, l.straat, l.huisnummer, t.Tafelnummer, t.aantalplaatsen, t.isbezet " +
                        $"from Restaurant r " +
                        $"left join locatie l on r.locatieid = l.id " +
                        $"left join tafel t on r.id = t.restaurantid " +
                        $"where r.id = {id}";
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (r == null)
                        {
                            int postcode = (int)reader["Postcode"];
                            string gemeente = (string)reader["Gemeente"];
                            string straat = reader["Straat"] == DBNull.Value ? null : (string)reader["Straat"];
                            string huisnummer = reader["Huisnummer"] == DBNull.Value ? null : (string)reader["Huisnummer"];
                            Keuken keuken = (Enum.Parse<Keuken>((string)reader["keuken"]));
                            string naam = (string)reader["naam"];
                            string email = (string)reader["Email"];
                            string telefoonnummer = (string)reader["telefoonnummer"];

                            l = new(postcode, gemeente, straat, huisnummer);
                            l.ZetId((int)reader["LocatieId"]);

                            r = new(naam, l, telefoonnummer, email, keuken);
                            r.ZetId((int)reader["RestaurantId"]);
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("Tafelnummer"))) // heeft tafels
                        {
                            tafelnummer = (int)reader["Tafelnummer"];
                            if (tafelnummer != tafelnummerOld)
                            {
                                // Nieuwe tafel of de eerste
                                if (tafelnummerOld > 0)
                                {
                                    // Maak tafel, einde bereikt
                                    Tafel t = new(tafelnummerOld, aantalPlaatsen, isBezet, id);
                                    r.VoegTafelToe(t);
                                }
                                first = true;
                                tafelnummerOld = tafelnummer;
                            }
                            if (first)
                            {
                                aantalPlaatsen = (int)reader["AantalPlaatsen"];
                                isBezet = (bool)reader["IsBezet"];
                                first = false;
                            }
                        }

                    }
                    reader.Close();
                    if (tafelnummer > 0)
                    {
                        Tafel t = new(tafelnummer, aantalPlaatsen, isBezet, id);
                        r.VoegTafelToe(t);
                    }
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

        public IReadOnlyList<Restaurant> GeefRestaurants(int? postcode, Keuken? keuken)
        {
            using SqlCommand cmd = _connection.CreateCommand();
            try
            {
                _connection.Open();
                List<Restaurant> restaurants = new();
                Dictionary<int, Locatie> locaties = new();
                int restaurantIdOld = -1;
                Restaurant r = null;
                Tafel t = null;
                if (postcode.HasValue && postcode.Value != 0 && keuken.HasValue)
                {
                    cmd.CommandText = $"select r.Id RestaurantId, r.naam, r.email, r.telefoonnummer, r.keuken, l.id locatieid, l.postcode, l.gemeente, l.straat, l.huisnummer, t.Tafelnummer, t.aantalplaatsen, t.isbezet " +
                        $"from Restaurant r " +
                        $"left join locatie l on r.locatieid = l.id " +
                        $"left join tafel t on r.id = t.restaurantid " +
                        $"where l.postcode = {postcode} and r.keuken = '{keuken}' and r.is_visible = 1 and t.is_visible = 1 " +
                        $"order by RestaurantId";
                }
                else if (postcode.HasValue && postcode.Value != 0)
                {
                    cmd.CommandText = $"select r.Id RestaurantId, r.naam, r.email, r.telefoonnummer, r.keuken, l.id locatieid, l.postcode, l.gemeente, l.straat, l.huisnummer, t.Tafelnummer, t.aantalplaatsen, t.isbezet " +
                        $"from Restaurant r " +
                        $"left join locatie l on r.locatieid = l.id " +
                        $"left join tafel t on r.id = t.restaurantid " +
                        $"where l.postcode = {postcode} and r.is_visible = 1 and t.is_visible = 1 " +
                        $"order by RestaurantId";
                }
                else if (keuken.HasValue)
                {
                    cmd.CommandText = $"select r.Id RestaurantId, r.naam, r.email, r.telefoonnummer, r.keuken, l.id locatieid, l.postcode, l.gemeente, l.straat, l.huisnummer, t.Tafelnummer, t.aantalplaatsen, t.isbezet " +
                        $"from Restaurant r " +
                        $"left join locatie l on r.locatieid = l.id " +
                        $"left join tafel t on r.id = t.restaurantid " +
                        $"where r.keuken = '{keuken}' and r.is_visible = 1 and t.is_visible = 1 " +
                        $"order by RestaurantId";
                }
                else throw new RestaurantRepositoryException("GeefRestaurants - repo - geen params");
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    int restaurantId = (int)reader["RestaurantId"];
                    if (restaurantId != restaurantIdOld)
                    {
                        Locatie l = null;
                        if (reader["LocatieId"] is int locId && !locaties.TryGetValue(locId, out l))
                        {
                            //TODO: get location fields
                            //int postcode = (int)reader["Postcode"];
                            string gemeente = (string)reader["Gemeente"];
                            string straat = reader["Straat"] == DBNull.Value ? null : (string)reader["Straat"];
                            string huisnummer = reader["Huisnummer"] == DBNull.Value ? null : (string)reader["Huisnummer"];
                            l = new(locId, (int)reader["postcode"], gemeente, straat, huisnummer);
                            locaties.Add(locId, l);
                        }

                        //TODO: get restaurant fields
                        //Keuken keuken = (Enum.Parse<Keuken>((string)reader["keuken"]));
                        string naam = (string)reader["naam"];
                        string email = (string)reader["Email"];
                        string telefoonnummer = (string)reader["telefoonnummer"];
                        r = new(restaurantId, naam, l, telefoonnummer, email, Enum.Parse<Keuken>((string)reader["keuken"]));
                        restaurants.Add(r);
                        restaurantIdOld = restaurantId;
                    }
                    if (reader["Tafelnummer"] is int tafelnummer)
                    {
                        //TODO: get table fields
                        // No need to test whether this is a new table. Tables are unique.
                        int aantalPlaatsen = (int)reader["AantalPlaatsen"];
                        bool isBezet = (bool)reader["IsBezet"];
                        t = new(tafelnummer, aantalPlaatsen, isBezet, restaurantId);
                        r.VoegTafelToe(t);
                    }
                }
                reader.Close();
                return restaurants;
            }
            catch (Exception ex)
            {
                throw new RestaurantRepositoryException("GeefRestaurants - repo", ex);
            }
            finally
            {
                _connection.Close();
            }
        }

        // ??
        public void UpdateRestaurant(Restaurant restaurant)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"update restaurant set naam = '{restaurant.Naam}', email = '{restaurant.Email}', telefoonnummer = '{restaurant.Telefoonnummer}', keuken = '{restaurant.Keuken}' where id = {restaurant.Id}";
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

        // ??
        public void VerwijderRestaurant(Restaurant restaurant)
        {
            using (SqlCommand cmd = _connection.CreateCommand())
            {
                try
                {
                    _connection.Open();
                    cmd.CommandText = $"update restaurant set is_visible = 0 where id = {restaurant.Id}";
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

        // ??
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
