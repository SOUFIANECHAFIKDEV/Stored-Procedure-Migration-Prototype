using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StoredProcedureMigrationPrototype.Shared.Migrations
{
    public static class DatabaseMigrationProcessorDepricated
    {
        /// <summary>
        /// Applies migrations for a single `DbContext` instance using the existing DI configuration.
        /// </summary>
        /// <typeparam name="TContext">The type of the DbContext.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        public static void ApplyMigrationsForDbContext<TContext>(IServiceProvider serviceProvider)
            where TContext : DbContext
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
            ProcessMigrations(dbContext);
        }

        /// <summary>
        /// Applies migrations for multiple registered `DbContext` instances in the DI container.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public static void ApplyMigrationsForAllDbContexts(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContexts = scope.ServiceProvider.GetServices<DbContext>();

            foreach (var dbContext in dbContexts)
            {
                ProcessMigrations(dbContext);
            }
        }

        /// <summary>
        /// Applies migrations for multiple databases based on connection strings specified in the configuration.
        /// </summary>
        /// <typeparam name="TContext">The type of the DbContext.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connectionStringKeys">An array of connection string keys from the configuration.</param>
        public static void ApplyMigrationsForDatabasesByConnectionStrings<TContext>(IServiceProvider serviceProvider, params string[] connectionStringKeys)
            where TContext : DbContext
        {
            using var scope = serviceProvider.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            foreach (var connectionStringKey in connectionStringKeys)
            {
                var connectionString = configuration.GetConnectionString(connectionStringKey);
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    Console.WriteLine($"Connection string for key '{connectionStringKey}' is null or empty. Skipping migration.");
                    continue;
                }

                Console.WriteLine($"Applying migrations for database: {connectionString}");
                ApplyMigrationsForSingleDatabase<TContext>(scope.ServiceProvider, connectionString);
            }
        }

        /// <summary>
        /// Applies migrations for a single database using a specified connection string.
        /// </summary>
        /// <typeparam name="TContext">The type of the DbContext.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connectionString">The connection string.</param>
        private static void ApplyMigrationsForSingleDatabase<TContext>(IServiceProvider serviceProvider, string connectionString)
            where TContext : DbContext
        {
            try
            {
                // Create DbContextOptions with the specified connection string
                var dbContextOptions = new DbContextOptionsBuilder<TContext>()
                    .UseSqlServer(connectionString)
                    .Options;

                // Create a new DbContext instance with the configured options
                using var dbContext = ActivatorUtilities.CreateInstance<TContext>(
                    serviceProvider,
                    dbContextOptions
                );

                ProcessMigrations(dbContext, connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying migrations for database: {connectionString}. Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Processes migrations for a specific `DbContext` instance.
        /// </summary>
        /// <param name="dbContext">The DbContext instance.</param>
        /// <param name="connectionString">The optional connection string for logging purposes.</param>
        private static void ProcessMigrations(DbContext dbContext, string? connectionString = null)
        {
            try
            {
                connectionString ??= dbContext.Database.GetDbConnection().ConnectionString;
                Console.WriteLine($"Starting migrations for database: {connectionString}");

                // Apply pending migrations
                dbContext.Database.Migrate();
                Console.WriteLine($"Migrations applied successfully for database: {connectionString}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying migrations for database: {connectionString}. Exception: {ex.Message}");
            }
        }
    }
}
