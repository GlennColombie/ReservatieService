using System.Data.SqlClient;
using ReservatieServiceBL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservatieServiceDL.Exceptions;
using ReservatieServiceBL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ReservatieServiceDL.Repositories
{
    public class ReservatieRepository : IReservatieRepository
    {
        private readonly ReservatieServiceContext _context;

        public ReservatieRepository(string connectionString)
        {
            _context = new (connectionString);
        }

        public bool BestaatReservatie(Reservatie reservatie)
        {
            try
            {
                return _context.Reservaties.Any(r => r.Gebruiker == reservatie.Gebruiker && r.Restaurant == reservatie.Restaurant && r.Datum == reservatie.Datum
                && r.Uur == reservatie.Uur && r.Einduur == reservatie.Einduur && r.Tafel == reservatie.Tafel && r.AantalPlaatsen == reservatie.AantalPlaatsen);
            }
            catch (Exception ex)
            {
                throw new ReservatieRepositoryException("BestaatReservatie", ex);
            }
        }
        
        public void VoegReservatieToe(Reservatie reservatie)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var g = _context.Gebruikers.Where(g => g.GebruikerId == reservatie.Gebruiker.GebruikerId).Include(g => g.Reservaties).Include(g => g.Locatie).FirstOrDefault();
                var r = _context.Restaurants.Where(r => r.Id == reservatie.Restaurant.Id).Include(r => r.Reservaties).Include(r => r.Locatie).FirstOrDefault();
                var t = _context.Tafels.Where(t => t.Tafelnummer == reservatie.Tafel.Tafelnummer && t.Restaurant.Id == reservatie.Restaurant.Id).Include(t => t.Restaurant).Include(t => t.Reservaties).FirstOrDefault();
                reservatie.Gebruiker = g;
                reservatie.ZetGebruikerId();
                reservatie.Restaurant = r;
                reservatie.ZetRestaurantId();
                reservatie.Tafel = t;
                reservatie.ZetTafelNummer();
                _context.Reservaties.Add(reservatie);
                SaveAndClear();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ReservatieRepositoryException("VoegReservatieToe", ex);
            }
        }

        public void AnnuleerReservatie(int reservatieNummer)
        {
            try
            {
                var r =  _context.Reservaties.Find(reservatieNummer);
                _context.Reservaties.Remove(r);
                SaveAndClear();
            }
            catch (Exception ex)
            {
                throw new ReservatieRepositoryException("AnnuleerReservatieAsync", ex);
            }
        }
        
        public void UpdateReservatie(Reservatie reservatie)
        {
            try
            {
                var r =  _context.Reservaties.Find(reservatie.Reservatienummer);
                r.ZetDatum(reservatie.Datum);
                r.ZetUur(reservatie.Uur);
                r.ZetEinduur();
                r.ZetAantalPlaatsen(reservatie.AantalPlaatsen);
                SaveAndClear();
            }
            catch (Exception ex)
            {
                throw new ReservatieRepositoryException("UpdateReservatieAsync", ex);
            }
        }

        public Reservatie GeefReservatie(int reservatienummer)
        {
            try
            {
                return _context.Reservaties
                    .Where(r => r.Reservatienummer == reservatienummer)
                    .Include(r => r.Gebruiker.Locatie)
                    .Include(r => r.Restaurant.Tafels)
                    .Include(r => r.Restaurant.Locatie)
                    .Include(r => r.Tafel.Restaurant)
                    .AsNoTracking().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ReservatieRepositoryException("GeefReservatie", ex);
            }
        }

        private void SaveAndClear()
        {
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }
    }
}
