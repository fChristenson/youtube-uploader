param(
    [Parameter(Mandatory = $true)]
    [string]$key,
    [Parameter(Mandatory = $true)]
    [string]$video
)

Write-Host "Running data gathering for video $video"

Write-Host slice
dotnet run -- slice -s 120 $video
Write-Host mp3
dotnet run -- mp3 slice.$video
Write-Host transcript
dotnet run -- transcript .\slice.$video.mp3 -k $key > transcript.txt

$transcript = Get-Content transcript.txt -Raw

Write-Host title
dotnet run -- title "$transcript" -k $key > title.txt
Write-Host description
dotnet run -- description "$transcript" -k $key > description.txt
Write-Host tags
dotnet run -- tags "$transcript" -k $key > tags.txt
