using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Exceptions
{
    public class RoomNotRentedException: ApplicationException
    {
        public RoomNotRentedException(string message)
            : base(message)
        {
            
        }

        public RoomNotRentedException(string message, Exception exception)
            :base(message,exception)
        {
            
        }
    }
}
