using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Entities;
using ReservatieServiceDL.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ReservatieServiceDL.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ReservatieServiceContext _context;
        public RestaurantRepository(string connectionString)
        {
            _context = new(connectionString);
        }
        private void SaveAndClear()
        {
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
        public bool BestaatRestaurant(Restaurant restaurant)
        {
            return _context.Restaurants.Any(r => r.Naam == restaurant.Naam && r.Email == restaurant.Email && r.Telefoonnummer == restaurant.Telefoonnummer && r.Locatie == restaurant.Locatie && r.Keuken == restaurant.Keuken);
        }

        public bool BestaatRestaurant(int id)
        {
            return _context.Restaurants.AsNoTracking().Any(r => r.Id == id);
        }

        public void UpdateRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            SaveAndClear();
        }

        public void VerwijderRestaurant(Restaurant restaurant)
        {
            _context.Restaurants.Remove(restaurant);
            SaveAndClear();
        }

        public void VoegRestaurantToe(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            _context.Entry(restaurant.Locatie).State = EntityState.Unchanged;
            SaveAndClear();
        }

        public IReadOnlyList<Restaurant> GeefRestaurants()
        {
            return _context.Restaurants.Where(r => r.IsVisible == 1).Include(r => r.Locatie).Include(r => r.Reservaties).AsNoTracking().ToList();
        }

        public Restaurant GeefRestaurant(int id)
        {
            return _context.Restaurants.Include(r => r.Locatie).Include(r => r.Tafels).Include(r => r.Reservaties).ThenInclude(r => r.Gebruiker.Locatie).Include(r => r.Reservaties).ThenInclude(r => r.Tafel).AsNoTracking().FirstOrDefault(r => r.Id == id);
        }

        public IReadOnlyList<Tafel> GeefAlleTafelsVanRestaurant(Restaurant restaurant)
        {
            return _context.Tafels.Where(t => t.RestaurantId == restaurant.Id && t.IsVisible == 1).AsNoTracking().ToList();
        }

        public IReadOnlyList<Restaurant> GeefRestaurants(int? postcode, Keuken? keuken)
        {
            return _context.Restaurants.Include(r => r.Locatie).Where(r => (postcode == null || r.Locatie.Postcode == postcode) && (keuken == null || r.Keuken == keuken)).AsNoTracking().ToList();
        }

        public void VoegTafelToe(Tafel tafel, Restaurant restaurant)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var r = _context.Restaurants.Find(restaurant.Id);
                    tafel.Restaurant = r;
                    tafel.ZetRestaurantId();
                    r.Tafels.Add(tafel);
                    _context.Tafels.Add(tafel);
                    SaveAndClear();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new RestaurantRepositoryException("VoegTafelToe - repo", ex);
                }
            }
        }
        public void VerwijderTafel(Tafel tafel, Restaurant restaurant)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    Tafel db = _context.Tafels.Find(tafel.Tafelnummer, restaurant.Id);
                    _context.Tafels.Remove(db);
                    SaveAndClear();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new RestaurantRepositoryException("VoegTafelToe - repo", ex);
                }
            }
        }
        public Tafel GeefTafel(int tafelnummer, Restaurant restaurant)
        {
            return _context.Tafels.Include(t => t.Restaurant).AsNoTracking().FirstOrDefault(t => t.Tafelnummer == tafelnummer);
        }
        public void UpdateTafel(Tafel tafel, Restaurant restaurant)
        {
            Tafel db = _context.Tafels.Find(tafel.Tafelnummer, restaurant.Id);
            db.ZetAantalPlaatsen(tafel.AantalPlaatsen);
            db.ZetTafelnummer(tafel.Tafelnummer);
            SaveAndClear();
        }
        public bool BestaatTafel(int tafelnummer, Restaurant restaurant)
        {
            return _context.Tafels.Any(t => t.Tafelnummer == tafelnummer && t.RestaurantId == restaurant.Id && t.IsVisible == 1);
        }
        public IReadOnlyList<Restaurant> GeefRestaurantsMetVrijeTafels(DateTime datum, int aantalPersonen)
        {
            try
            {
                var r = _context.Restaurants
                    .Include(r => r.Locatie)
                    .ToList();
                foreach (var a in r)
                {
                    _context.Entry(a)
                        .Collection(b => b.Tafels)
                        .Query().OrderBy(t => t.AantalPlaatsen).Where(t => t.AantalPlaatsen >= aantalPersonen && !t.Reservaties.Any(res => res.Uur.AddHours(-1.5) >= datum || res.Einduur <= datum)).Take(1)
                        .Load();
                }
                return r;
            }
            catch (Exception ex)
            {
                throw new RestaurantRepositoryException("GeefRestaurantsMetVrijeTafels - repo", ex);
            }
        }
        public IReadOnlyList<Restaurant> GeefRestaurantsMetVrijeTafels(DateTime datum, int aantalPersonen, int postcode)
        {
            try
            {
                var r = _context.Restaurants
                    .Include(r => r.Locatie)
                    .Where(r => r.Locatie.Postcode == postcode)
                    .ToList();
                foreach (var a in r)
                {
                    _context.Entry(a)
                        .Collection(b => b.Tafels)
                        .Query().OrderByDescending(t => t.Tafelnummer).Where(t => t.AantalPlaatsen >= aantalPersonen && !t.Reservaties.Any(res => res.Uur.AddHours(-1.5) >= datum || res.Einduur <= datum)).Take(1)
                        .Load();
                }
                return r;
            }
            catch (Exception ex)
            {
                throw new RestaurantRepositoryException("GeefRestaurantsMetVrijeTafels - repo", ex);
            }
        }
        public IReadOnlyList<Restaurant> GeefRestaurantsMetVrijeTafels(DateTime datum, int aantalPersonen, Keuken keuken)
        {
            try
            {
                var r = _context.Restaurants
                    .Include(r => r.Locatie)
                    .Where(r => r.Keuken == keuken)
                    .ToList();
                foreach (var a in r)
                {
                    _context.Entry(a)
                        .Collection(b => b.Tafels)
                        .Query().OrderByDescending(t => t.Tafelnummer).Where(t => t.AantalPlaatsen >= aantalPersonen && !t.Reservaties.Any(res => res.Uur.AddHours(-1.5) >= datum || res.Einduur <= datum)).Take(1)
                        .Load();
                }
                return r;
            }
            catch (Exception ex)
            {
                throw new RestaurantRepositoryException("GeefRestaurantsMetVrijeTafels - repo", ex);
            }
        }
        public IReadOnlyList<Restaurant> GeefRestaurantsMetVrijeTafels(DateTime datum, int aantalPersonen, int postcode, Keuken keuken)
        {
            try
            {
                var r = _context.Restaurants
                    .Include(r => r.Locatie)
                    .Where(r => r.Locatie.Postcode == postcode && r.Keuken == keuken)
                    .ToList();
                foreach (var a in r)
                {
                    _context.Entry(a)
                        .Collection(b => b.Tafels)
                        .Query().OrderByDescending(t => t.Tafelnummer).Where(t => t.AantalPlaatsen >= aantalPersonen && !t.Reservaties.Any(res => res.Uur.AddHours(-1.5) >= datum || res.Einduur <= datum)).Take(1)
                        .Load();
                }
                return r;
            }
            catch (Exception ex)
            {
                throw new RestaurantRepositoryException("GeefRestaurantsMetVrijeTafels - repo", ex);
            }
        }
        public IReadOnlyList<Reservatie> GeefReservatiesRestaurant(Restaurant restaurant, DateTime datum)
        {
            return _context.Reservaties.Where(r => r.Restaurant == restaurant && r.Datum == datum).Include(r => r.Gebruiker.Locatie).Include(r => r.Tafel).AsNoTracking().ToList();
        }
        public IReadOnlyList<Reservatie> GeefReservatiesRestaurant(Restaurant restaurant, DateTime datum, DateTime einddatum)
        {
            return _context.Reservaties.Where(r => r.Restaurant == restaurant && r.Datum >= datum && r.Datum <= einddatum).Include(r => r.Gebruiker.Locatie).Include(r => r.Tafel).AsNoTracking().ToList();
        }
    }
}
