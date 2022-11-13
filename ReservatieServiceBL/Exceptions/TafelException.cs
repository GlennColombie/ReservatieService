using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservatieServiceBL.Exceptions
{
    public class TafelException : Exception
    {
        public TafelException(string message) : base(message)
        {
        }
    }
}
