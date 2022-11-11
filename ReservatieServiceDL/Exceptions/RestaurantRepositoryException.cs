using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservatieServiceDL.Exceptions
{
    public class RestaurantRepositoryException : Exception
    {
        public RestaurantRepositoryException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
