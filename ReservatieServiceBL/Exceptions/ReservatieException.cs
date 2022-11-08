using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservatieServiceBL.Exceptions
{
    public class ReservatieException : Exception
    {
        public ReservatieException(string message) : base(message)
        {
        }
    }
}