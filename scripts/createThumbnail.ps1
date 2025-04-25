param(
    [Parameter(Mandatory = $true)]
    [string]$key,
    [Parameter(Mandatory = $true)]
    [string]$video,
    [Parameter(Mandatory = $true)]
    [string]$transcript
)

Write-Output "Running thumbnail creation for video $video"

# Run the first two commands as before
dotnet run -- thumbnail still -o 3 $video

# Run the thumbnail text command, and save the output into a file
dotnet run -- thumbnail text $transcript -k $key | Set-Content -Path thumbnail.text.txt

# Get the content from the file, ensuring special characters are properly handled
$text = Get-Content .\thumbnail.text.txt -Raw

# Remove extra quotes (if any) and avoid PowerShell special character issues
# No need for escaping quotes, just pass the plain text as it is
$escapedText = $text.Trim('"')  # Removes quotes if any are present at the start/end

# Run the final command with the clean text
dotnet run -- thumbnail image -s "still.$video.jpg" -t "$escapedText"
