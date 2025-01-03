using Microsoft.EntityFrameworkCore;
using StoredProcedureMigrationPrototype.API2.DataAccessLayer.Context;
using StoredProcedureMigrationPrototype.API2.DataAccessLayer.Mappings;
using StoredProcedureMigrationPrototype.Shared.Migrations;
using StoredProcedureMigrationPrototype.Shared.Procedures;

var builder = WebApplication.CreateBuilder(args);

// Register AutoMapper with all profiles
builder.Services.AddAutoMapper(typeof(OrderProfile));

// Add DbContext for API2
builder.Services.AddDbContext<Api2DbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Api2Database")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

#region Apply Migrations For Database During Project Startup
// Retrieve migration configuration settings AutoMigrationSettings.MigrationsSettings.EnableAutoMigration
var EnableAutoMigration = builder.Configuration.GetValue<bool>("AutoMigrationSettings:MigrationsSettings:EnableAutoMigration");
var migrationsPath = builder.Configuration.GetValue<string>("AutoMigrationSettings:MigrationsSettings:MigrationsPath");

if (EnableAutoMigration && !string.IsNullOrWhiteSpace(migrationsPath))
{
    DatabaseMigrationProcessor.GenerateAndApplyMigrationIfPending<Api2DbContext>(
        app.Services,
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, migrationsPath)
    );
}
#endregion Apply Migrations For Multiple Databases

#region Process Stored Procedures During Startup
// Retrieve stored procedure configuration settings
var EnableAutoMigrationForStoredProcedure = builder.Configuration.GetValue<bool>("AutoMigrationSettings:StoredProcedureSettings:EnableAutoMigration");
var storedProceduresFolderPath = builder.Configuration.GetValue<string>("AutoMigrationSettings:StoredProcedureSettings:ProceduresFolderPath");

// Check if migration is enabled and the folder path is configured
if (EnableAutoMigrationForStoredProcedure && !string.IsNullOrWhiteSpace(storedProceduresFolderPath))
{
    StoredProcedureProcessor.ProcessStoredProcedures<Api2DbContext>(
        app.Services,
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, storedProceduresFolderPath)
    );
}
#endregion Process Stored Procedures During Startup

app.MapControllers();
app.Run();
