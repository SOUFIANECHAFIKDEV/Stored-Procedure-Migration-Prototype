using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Migrations
{
    public static class MigrationHelperg
    {
        public static void ApplyMigrationsForDatabased<TContext>(IServiceProvider serviceProvider, string connectionString) where TContext : DbContext
        {
            try
            {
                var dbContextOptions = new DbContextOptionsBuilder<TContext>()
                    .UseSqlServer(connectionString)
                    .Options;

                using var dbContext = ActivatorUtilities.CreateInstance<TContext>(
                    serviceProvider,
                    dbContextOptions
                );

                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying migrations: {ex.Message}");
            }
        }
    }
}
