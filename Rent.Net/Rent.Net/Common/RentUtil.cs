using Rent.Net.Entities;

namespace Rent.Net.Common
{
    public static class RentUtil
    {
        public static readonly RentDbContext Database = new RentDbContext();
    }
}