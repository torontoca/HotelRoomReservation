using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Utils
{
    public static class EnumerableSupport
    {
        public static bool HasAnyElement<T>(this IEnumerable<T> list)
        {
            return (list != null && list.Count() != 0);
        }
    }
}
