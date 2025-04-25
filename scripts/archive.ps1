param (
    [Parameter(Mandatory = $true)]
    [string]$DestinationFolder,

    [Parameter(Mandatory = $true)]
    [string]$CurrentVideo
)

# Create the destination folder if it doesn't exist
if (-not (Test-Path -Path $DestinationFolder)) {
    New-Item -ItemType Directory -Path $DestinationFolder | Out-Null
}

# File extensions to move
$fileExtensions = @("*.txt", "*.mp3", "*.jpg", "*.png", "*.mp4")

# Move files by extension
foreach ($ext in $fileExtensions) {
    Get-ChildItem -Path . -Filter $ext -File | ForEach-Object {
        $destinationPath = Join-Path -Path $DestinationFolder -ChildPath $_.Name
        Move-Item -Path $_.FullName -Destination $destinationPath -Force
    }
}

# Move the current video file if it exists
if (Test-Path -Path $CurrentVideo) {
    $videoFile = Get-Item -Path $CurrentVideo
    $videoDest = Join-Path -Path $DestinationFolder -ChildPath $videoFile.Name
    Move-Item -Path $videoFile.FullName -Destination $videoDest -Force
    Write-Host "Moved current video file '$CurrentVideo' to '$DestinationFolder'"
}
else {
    Write-Warning "Current video file '$CurrentVideo' not found."
}

Write-Host "All matching files moved to '$DestinationFolder'"
