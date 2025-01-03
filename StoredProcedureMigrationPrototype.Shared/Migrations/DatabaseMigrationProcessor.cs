using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace StoredProcedureMigrationPrototype.Shared.Migrations
{
    /// <summary>
    /// Provides utilities for managing EF Core database migrations.
    /// </summary>
    public static class DatabaseMigrationProcessor
    {
        /// <summary>
        /// Automatically detects pending changes in the EF Core model, generates a migration if necessary,
        /// and applies it to the database. Migration files are saved to the specified path.
        /// </summary>
        /// <typeparam name="TContext">The type of the DbContext to process migrations for.</typeparam>
        /// <param name="serviceProvider">The service provider to resolve dependencies, including the DbContext.</param>
        /// <param name="migrationsPath">The file system path where migration files should be stored.</param>
        /// <remarks>
        /// - This method assumes that the application's service provider has been configured
        ///   to include the specified DbContext (`TContext`) and required EF Core services.
        /// - Ensure the specified `migrationsPath` is accessible and writable by the application.
        /// - Catch blocks handle any errors that may occur during migration generation or application.
        /// </remarks>
        /// <example>
        /// Usage:
        /// <code>
        /// var serviceProvider = new ServiceCollection()
        ///     .AddDbContext<MyDbContext>(options => options.UseSqlServer(connectionString))
        ///     .BuildServiceProvider();
        /// 
        /// DatabaseMigrationProcessor.GenerateAndApplyMigrationIfPending<MyDbContext>(serviceProvider, "./Migrations");
        /// </code>
        /// </example>
        public static void GenerateAndApplyMigrationIfPending<TContext>(IServiceProvider serviceProvider, string migrationsPath)
            where TContext : DbContext
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

            try
            {
                // Check for pending migrations
                var pendingMigrations = dbContext.Database.GetPendingMigrations();
                if (!pendingMigrations.Any())
                {
                    Console.WriteLine("No pending model changes detected. Skipping migration generation.");
                    return;
                }

                // Apply the migration to the database
                Console.WriteLine("Applying migration to the database...");
                dbContext.Database.Migrate();
                Console.WriteLine("Migration applied successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating or applying migration: {ex.Message}");
            }
        }
    }
}
