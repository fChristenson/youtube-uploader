param (
    [Parameter(Mandatory = $true)]
    [datetime]$startDate,
    [Parameter(Mandatory = $true)]
    [string]$key,
    [Parameter(Mandatory = $true)]
    [string]$clientId,
    [Parameter(Mandatory = $true)]
    [string]$clientSecret
)

# Get all .mkv files sorted by name
$mkvFiles = Get-ChildItem -Path . -File | Where-Object { $_.Name.ToLower().EndsWith(".mkv") } | Sort-Object Name

# Iterate over each file and increment the date
$index = 0
foreach ($video in $mkvFiles) {
    try {
        $scheduledDate = $startDate.AddDays($index)
        $releaseDateTime = $scheduledDate.ToString("yyyy-MM-ddTHH:mm:ss")
    
        Write-Host "Processing $($video.Name) - $releaseDateTime"

        ./scripts/upload.ps1 `
            -key $key `
            -clientId $clientId `
            -clientSecret $clientSecret `
            -releaseDateTime $releaseDateTime `
            -video $video
        
        # Create the destination folder if it doesn't exist
        if (-not (Test-Path -Path "dist")) {
            New-Item -ItemType Directory -Path "dist" | Out-Null
        }

        ./scripts/archive.ps1 -DestinationFolder dist/$video
        $index++
    }
    catch {
        Write-Host "Error: $($_.Exception.Message)"
        Write-Host "StackTrace: $($_.ScriptStackTrace)"
        exit 1
    }
}
