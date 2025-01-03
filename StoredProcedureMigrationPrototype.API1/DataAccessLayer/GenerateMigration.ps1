#========================================
# Auto-Migration Script for EF Core
#========================================
# Instructions:
# - Configure the name of your DbContext by setting the $dbContext variable below.
# - Execute this script from the project root directory:
#   ./DataAccessLayer/GenerateMigration.ps1
#========================================

# Configure your DbContext name here
$dbContext = "Api1DbContext"

# Get the current directory
$currentDir = Get-Location

# Define the path to the parent project directory
$projectDir = "$currentDir\.."

# Detect the .csproj file in the parent directory
$projectFile = Get-ChildItem -Path $projectDir -Recurse -Filter *.csproj | Select-Object -First 1

# Validate the .csproj file exists
if (-not $projectFile) {
    Write-Host "No .csproj file found in the parent directory or subdirectories: $projectDir" -ForegroundColor Red
    exit 1
}

# Generate a unique migration name using a timestamp
$migrationName = "_Migration"

# Define the output directory for migrations relative to the DataAccessLayer
$outputDir = "DataAccessLayer/Migrations"

# Construct the EF Core migration command
$command = "dotnet ef migrations add $migrationName --context $dbContext --output-dir $outputDir --project $($projectFile.FullName)"

# Display the generated command for debugging
Write-Host "`nGenerated Command:" -ForegroundColor Cyan
Write-Host $command -ForegroundColor Green

# Execute the generated command
try {
    Invoke-Expression $command
    Write-Host "`nMigration created successfully!" -ForegroundColor Green
} catch {
    Write-Host "`nError during migration creation:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}
