using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace StoredProcedureMigrationPrototype.Shared.Procedures
{
    public static class StoredProcedureProcessor
    {
        /// <summary>
        /// Automatically processes all stored procedures in the specified path during startup.
        /// </summary>
        /// <typeparam name="TContext">The DbContext type.</typeparam>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="proceduresFolderPath">The path to the folder containing stored procedures.</param>
        public static void ProcessStoredProcedures<TContext>(IServiceProvider serviceProvider, string proceduresFolderPath)
            where TContext : DbContext
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();

                // Ensure the __StoredProceduresHistory table exists
                StoredProcedureManager.EnsureHistoryTableExists(dbContext);

                // Verify the folder path exists
                if (!Directory.Exists(proceduresFolderPath))
                {
                    Console.WriteLine($"Procedures directory not found: {proceduresFolderPath}");
                    return;
                }

                using (var connection = dbContext.Database.GetDbConnection())
                {
                    connection.Open(); // Ensure the connection is open

                    // Iterate through each stored procedure folder
                    foreach (var procedureFolder in Directory.GetDirectories(proceduresFolderPath))
                    {
                        var procedureName = Path.GetFileName(procedureFolder);
                        var scriptPath = Path.Combine(procedureFolder, $"{procedureName}.sql");

                        if (File.Exists(scriptPath))
                        {
                            StoredProcedureManager.ProcessStoredProcedure(dbContext, connection, procedureName, scriptPath);
                        }
                        else
                        {
                            Console.WriteLine($"Script not found for stored procedure: {procedureName}");
                        }
                    }
                }
            }
        }
    }
}
