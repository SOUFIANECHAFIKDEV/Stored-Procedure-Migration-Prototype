using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using StoredProcedureMigrationPrototype.Data.Attributes;
using StoredProcedureMigrationPrototype.Data.Models.Api1.Model;

namespace StoredProcedureMigrationPrototype.API1.Data
{
    public class Api1DbContext : DbContext
    {
        public Api1DbContext(DbContextOptions<Api1DbContext> options) : base(options)
        {
        }

        // DbSet for API1 entities
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierDetail> SupplierDetail { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Automatically exclude entities marked with [ExcludeFromMigrations]
            // Tell EF Core that the 'Products' table already exists in the database
            //modelBuilder.Entity<Product>().ToTable("Products").Metadata.SetIsTableExcludedFromMigrations(true);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var clrType = entityType.ClrType;

                if (clrType.GetCustomAttributes(typeof(ExcludeFromMigrationsAttribute), true).Any())
                {
                    entityType.SetIsTableExcludedFromMigrations(true);
                }
            }
            #endregion

            #region Additional configurations
            modelBuilder.Entity<Product>()
               .Property(p => p.DateAdded)
               .HasDefaultValueSql("GETDATE()") // Use SQL Server's GETDATE() as the default value
               .ValueGeneratedOnAdd();          // Ensures the value is generated only on insert

            modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,4)"); // Adjust precision and scale as needed
            #endregion
        }

    }
}
