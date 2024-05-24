# Define the source and destination directories
$sourceDirectory = "C:\Users\AsRock\source\repos\time.com\BookingApp\file\"
$destinationDirectory = "E:\backup\"
$databaseFile = "C:\Users\AsRock\source\repos\time.com\BookingApp\booking.db"
$backupDatabaseFile = Join-Path $destinationDirectory "booking.db"
$date = Get-Date -Format "yyyy-MM-dd"
$zipFileName = "backup_$date.zip"
$zipFilePath = Join-Path $destinationDirectory $zipFileName

# Create backup directory if it doesn't exist
if (-not (Test-Path -Path $destinationDirectory)) {
    New-Item -ItemType Directory -Path $destinationDirectory
}

# Create a temporary directory for the backup
$tempBackupDirectory = New-Item -ItemType Directory -Path (Join-Path $destinationDirectory "temp_backup")

try {
    # Copy files from the source directory to the temporary backup directory
    Get-ChildItem -Path $sourceDirectory -File | ForEach-Object {
        Copy-Item -Path $_.FullName -Destination $tempBackupDirectory.FullName -Force
    }

    # Copy the database file to the temporary backup directory
    Copy-Item -Path $databaseFile -Destination $tempBackupDirectory.FullName -Force

    # Compress the backup into a ZIP file
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [System.IO.Compression.ZipFile]::CreateFromDirectory($tempBackupDirectory.FullName, $zipFilePath)

    Write-Output "Backup and compression completed successfully."
}
catch {
    Write-Output "Error during backup: $_"
}
finally {
    # Remove the temporary backup directory
    Remove-Item -Path $tempBackupDirectory.FullName -Recurse -Force
}