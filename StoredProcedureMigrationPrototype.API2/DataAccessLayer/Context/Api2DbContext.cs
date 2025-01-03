using Microsoft.EntityFrameworkCore;
using StoredProcedureMigrationPrototype.Data.Attributes;
using StoredProcedureMigrationPrototype.Data.Models.Api1.Model;
using StoredProcedureMigrationPrototype.Data.Models.Api2.Model;

namespace StoredProcedureMigrationPrototype.API2.DataAccessLayer.Context
{
    public class Api2DbContext : DbContext
    {
        public Api2DbContext(DbContextOptions<Api2DbContext> options) : base(options)
        {
        }

        // DbSet for API2 entities
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Automatically exclude entities marked with [ExcludeFromMigrations]
            // Tell EF Core that the 'Products' table already exists in the database
            //modelBuilder.Entity<Order>().ToTable("Orders").Metadata.SetIsTableExcludedFromMigrations(true);

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

            #endregion
        }
    }
}
