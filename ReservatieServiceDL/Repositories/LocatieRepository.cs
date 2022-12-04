using ReservatieServiceBL.Interfaces;
using ReservatieServiceBL.Entities;
using ReservatieServiceDL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ReservatieServiceDL.Repositories
{
    public class LocatieRepository : ILocatieRepository
    {
        private readonly ReservatieServiceContext _context;
        public LocatieRepository(string connectionString)
        {
            _context = new(connectionString);
        }
        public bool BestaatLocatie(Locatie locatie)
        {
            if (locatie == null) throw new LocatieRepositoryException("BestaatLocatie - null");
            try
            {
                return _context.Locaties.Any(l => l.Postcode == locatie.Postcode && l.Gemeente == locatie.Gemeente && l.Straat == locatie.Straat && l.Huisnummer == locatie.Huisnummer);
            }
            catch (Exception ex)
            {
                throw new LocatieRepositoryException("BestaatLocatie - repo", ex);
            }
        }

        public void UpdateLocatie(Locatie locatie)
        {
            if (locatie == null) throw new LocatieRepositoryException("BestaatLocatie - null");
            try
            {
                _context.Locaties.Update(locatie);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new LocatieRepositoryException("BestaatLocatie - repo", ex);
            }
        }

        public void VerwijderLocatie(Locatie locatie)
        {
            if (locatie == null) throw new LocatieRepositoryException("BestaatLocatie - null");
            try
            {
                _context.Locaties.Remove(locatie);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new LocatieRepositoryException("BestaatLocatie - repo", ex);
            }
        }

        public void VoegLocatieToe(Locatie locatie)
        {
            if (locatie == null) throw new LocatieRepositoryException("BestaatLocatie - null");
            try
            {
                _context.Locaties.Add(locatie);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new LocatieRepositoryException("BestaatLocatie - repo", ex);
            }
        }

        public Locatie GeefLocatie(Locatie locatie)
        {
            try
            {
                return _context.Locaties.Where(l => l.Postcode == locatie.Postcode && l.Gemeente == locatie.Gemeente && l.Straat == locatie.Straat && l.Huisnummer == locatie.Huisnummer).AsNoTracking().FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new LocatieRepositoryException("GeefLocatie - repo", ex);
            }
        }
    }
}
