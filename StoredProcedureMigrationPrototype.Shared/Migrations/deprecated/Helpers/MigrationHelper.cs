using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace StoredProcedureMigrationPrototype.Data.Helpers
{
    public static class MigrationHelper
    {
        /// <summary>
        /// Applies pending migrations for multiple DbContexts.
        /// </summary>
        /// <param name="serviceProvider">The service provider from the application's DI container.</param>
        /// <param name="dbContexts">The list of DbContext types to migrate.</param>
        public static void ApplyMigrations(IServiceProvider serviceProvider, params Type[] dbContexts)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                foreach (var dbContextType in dbContexts)
                {
                    //var dbContext = scope.ServiceProvider.GetRequiredService(dbContextType) as DbContext;
                    var dbContext = scope.ServiceProvider.GetRequiredService(dbContextType) as DbContext;

                    if (dbContext != null)
                    {
                        try
                        {
                             dbContext.Database.Migrate(); // Apply migrations
                            Console.WriteLine($"Migrations applied successfully for {dbContextType.Name}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error applying migrations for {dbContextType.Name}: {ex.Message}");
                        }
                    }
                }
            }
        }
    }
}
