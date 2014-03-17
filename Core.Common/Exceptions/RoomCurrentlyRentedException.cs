using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    public class RoomCurrentlyRentedException : ApplicationException
    {
        public RoomCurrentlyRentedException(string message)
            : base(message)
        {

        }

        public RoomCurrentlyRentedException(string message, Exception exception)
            : base(message, exception)
        {

        }
    }

    
}
