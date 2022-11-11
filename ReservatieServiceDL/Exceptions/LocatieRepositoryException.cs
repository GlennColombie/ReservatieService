using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservatieServiceDL.Exceptions
{
    public class LocatieRepositoryException : Exception
    {
        public LocatieRepositoryException(string? message) : base(message)
        {
        }

        public LocatieRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
