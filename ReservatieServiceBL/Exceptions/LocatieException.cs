using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservatieServiceBL.Exceptions
{
    public class LocatieException : Exception
    {
        public LocatieException(string message) : base(message)
        {
        }
    }
}