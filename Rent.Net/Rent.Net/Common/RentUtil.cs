using Rent.Net.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Rent.Net.Common
{
    public static class RentUtil
    {
        public static bool IsNullOrEmpty<T>(this IList<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}