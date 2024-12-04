using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class BaseDbContext
        :DbContext
    {
        protected BaseDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MapSalesItem(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public virtual void MapSalesItem(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SalesItem>(clazz =>
            {
                clazz.ToTable("Sales");
                clazz.HasNoKey();
                // dimensions
                clazz.Property(x => x.Country).HasColumnName("Country");
                clazz.Property(x => x.Company).HasColumnName("Company");
                clazz.Property(x => x.Region).HasColumnName("Region");
                // measures
                clazz.Property(x => x.Sales).HasColumnName("Sales");
            });
        }

        public DbSet<SalesItem> Sales
        {
            get { return Set<SalesItem>(); }
        }
    }
}
