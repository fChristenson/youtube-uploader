param (
    [Parameter(Mandatory = $true)]
    [string]$DestinationFolder
)

# Create the destination folder if it doesn't exist
if (-not (Test-Path -Path $DestinationFolder)) {
    New-Item -ItemType Directory -Path $DestinationFolder | Out-Null
}

# File extensions to move
$fileExtensions = @("*.txt", "*.mp3", "*.jpg", "*.png", "*.mkv", "*.mp4")

foreach ($ext in $fileExtensions) {
    Get-ChildItem -Path . -Filter $ext -File | ForEach-Object {
        $destinationPath = Join-Path -Path $DestinationFolder -ChildPath $_.Name
        Move-Item -Path $_.FullName -Destination $destinationPath -Force
    }
}

Write-Host "All matching files moved to '$DestinationFolder'"
