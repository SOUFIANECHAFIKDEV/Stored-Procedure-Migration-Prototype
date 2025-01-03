#========================================
# Generate Migration Script
#========================================
# Instructions:
# - Run this script to generate migrations for the correct DbContext and project.
# - Ensure the script is executed in the correct project directory.
# - The generated migration files will be saved in the DataAccessLayer/Migrations folder.
#========================================

# Configure your DbContext name here
$dbContext = "Api2DbContext"

# Get the current directory
$currentDir = Get-Location

# Define the path to the MigrationScripts folder (optional)
$migrationScriptsDir = "$currentDir\DataAccessLayer\MigrationScripts"

# Ensure MigrationScripts folder exists
if (-not (Test-Path $migrationScriptsDir)) {
    New-Item -ItemType Directory -Path $migrationScriptsDir | Out-Null
}

# Detect the .csproj file in the current directory
$projectFile = Get-ChildItem -Path $currentDir -Recurse -Filter *.csproj | Select-Object -First 1
if (-not $projectFile) {
    Write-Host "No .csproj file found in the current directory or subdirectories: $currentDir" -ForegroundColor Red
    exit 1
}

# Remove the .csproj extension from the project path
$projectPath = Split-Path -Parent $projectFile.FullName

# Generate a unique migration name
$migrationName = "_Auto_Migration"

# Define the output directory for migrations
$outputDir = "DataAccessLayer/Migrations"

# Construct the EF Core migration command
$command = "dotnet ef migrations add $migrationName --context $dbContext --output-dir $outputDir --project $projectPath"

# Display the generated command for debugging
Write-Host "Generated Command:" -ForegroundColor Cyan
Write-Host $command -ForegroundColor Green

# Execute the command
try {
    Invoke-Expression $command
    Write-Host "Migration created successfully!" -ForegroundColor Green
} catch {
    Write-Host "Error creating migration:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}
