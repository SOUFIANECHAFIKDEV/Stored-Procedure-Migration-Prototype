#========================================
# Rollback Script for EF Core Migrations
#========================================
# Instructions:
# - Configure the name of your DbContext by setting the $dbContext variable below.
# - Place this script in the DataAccessLayer directory.
# - Execute this script from the project root directory:
#   ./DataAccessLayer/RollbackMigration.ps1
# - Specify the target migration name (optional) when running the script.
#   Example: ./DataAccessLayer/RollbackMigration.ps1 "PreviousMigrationName"
# - If no migration is specified, the database will be reverted to the last applied migration.
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

# Accept an optional migration name from the command line
if ($args.Length -gt 0) {
    $targetMigration = $args[0]
    Write-Host "`nTarget Migration: $targetMigration" -ForegroundColor Cyan
} else {
    $targetMigration = "0" # Default to rolling back all migrations
    Write-Host "`nNo target migration specified. Rolling back all migrations." -ForegroundColor Yellow
}

# Construct the EF Core rollback command
$command = "dotnet ef database update $targetMigration --context $dbContext --project $($projectFile.FullName)"

# Display the generated command for debugging
Write-Host "`nGenerated Command:" -ForegroundColor Cyan
Write-Host $command -ForegroundColor Green

# Execute the rollback command
try {
    Invoke-Expression $command
    Write-Host "`nDatabase rolled back successfully to migration: $targetMigration" -ForegroundColor Green
} catch {
    Write-Host "`nError during rollback:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
}
