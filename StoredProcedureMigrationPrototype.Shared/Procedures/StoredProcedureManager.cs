using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace StoredProcedureMigrationPrototype.Shared.Procedures
{
    public static class StoredProcedureManager
    {
        private const string HistoryTableName = "__StoredProceduresHistory";

        /// <summary>
        /// Ensure the history table exists in the database.
        /// </summary>
        public static void EnsureHistoryTableExists(DbContext dbContext)
        {
            var createTableSql = $@"
            IF NOT EXISTS (SELECT * FROM sysobjects WHERE name = '{HistoryTableName}' AND xtype = 'U')
            BEGIN
                CREATE TABLE {HistoryTableName} (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    ProcedureName NVARCHAR(255) NOT NULL,
                    ScriptHash NVARCHAR(64) NOT NULL,
                    AppliedOn DATETIME NOT NULL DEFAULT GETDATE(),
                    ScriptContent NVARCHAR(MAX) NULL
                )
            END";

            dbContext.Database.ExecuteSqlRaw(createTableSql);
        }

        /// <summary>
        /// Process a stored procedure: apply the script if there are changes.
        /// </summary>
        public static void ProcessStoredProcedure(DbContext dbContext, System.Data.Common.DbConnection connection, string procedureName, string scriptPath)
        {
            if (!File.Exists(scriptPath))
            {
                throw new FileNotFoundException($"Script file not found: {scriptPath}");
            }

            // Read the script content
            var scriptContent = File.ReadAllText(scriptPath);

            // Compute the hash of the script content
            var scriptHash = ComputeScriptHash(scriptContent);

            // Check if the stored procedure with the same hash has already been applied
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                SELECT COUNT(1)
                FROM __StoredProceduresHistory
                WHERE ProcedureName = @procedureName AND ScriptHash = @scriptHash";

                // Add parameters to prevent SQL injection
                var procedureNameParam = command.CreateParameter();
                procedureNameParam.ParameterName = "@procedureName";
                procedureNameParam.Value = procedureName;
                command.Parameters.Add(procedureNameParam);

                var scriptHashParam = command.CreateParameter();
                scriptHashParam.ParameterName = "@scriptHash";
                scriptHashParam.Value = scriptHash;
                command.Parameters.Add(scriptHashParam);

                // Execute the query to check if the stored procedure has already been applied
                var alreadyApplied = Convert.ToInt32(command.ExecuteScalar());

                if (alreadyApplied > 0)
                {
                    Console.WriteLine($"No changes detected for stored procedure {procedureName}. Skipping migration.");
                    return;
                }
            }

            // Apply the stored procedure script
            dbContext.Database.ExecuteSqlRaw(scriptContent);

            // Insert a record into the history table for the new version
            dbContext.Database.ExecuteSqlRaw(
                $"INSERT INTO __StoredProceduresHistory (ProcedureName, ScriptHash, ScriptContent) VALUES (@procedureName, @scriptHash, @scriptContent)",
                new SqlParameter("@procedureName", procedureName),
                new SqlParameter("@scriptHash", scriptHash),
                new SqlParameter("@scriptContent", scriptContent)
            );

            Console.WriteLine($"Applied new version of stored procedure {procedureName}.");
        }

        /// <summary>
        /// Compute the hash of a script's content for change detection.
        /// </summary>
        private static string ComputeScriptHash(string scriptContent)
        {
            using var md5 = MD5.Create();
            var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(scriptContent));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}
