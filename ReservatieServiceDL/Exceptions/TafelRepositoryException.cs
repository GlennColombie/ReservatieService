using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservatieServiceDL.Exceptions
{
    public class TafelRepositoryException : Exception
    {
        public TafelRepositoryException(string? message) : base(message)
        {
        }
    }
}
