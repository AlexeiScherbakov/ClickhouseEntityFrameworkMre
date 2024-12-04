using Database;
using Microsoft.EntityFrameworkCore;

namespace DatabaseClickhouse
{
    public class ClickhouseDbContext
         : BaseDbContext
    {
        public ClickhouseDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
