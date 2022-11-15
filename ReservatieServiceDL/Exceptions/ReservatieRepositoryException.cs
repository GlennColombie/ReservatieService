using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservatieServiceDL.Exceptions
{
    public class ReservatieRepositoryException : Exception
    {
        public ReservatieRepositoryException(string? message) : base(message)
        {
        }

        public ReservatieRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
