param(
    [Parameter(Mandatory = $true)]
    [string]$key,
    [Parameter(Mandatory = $true)]
    [string]$clientId,
    [Parameter(Mandatory = $true)]
    [string]$clientSecret,
    [Parameter(Mandatory = $true)]
    [string]$releaseDateTime,
    [Parameter(Mandatory = $true)]
    [string]$video
)

$distVideo = "youtube.$video.mp4"
$animatedVideo = "animated.$distVideo"
$thumbnail = "thumbnail.still.$distVideo.jpg"

dotnet run video optimize $video
./scripts/gatherVideoData.ps1 -key $key -video $video

$transcript = Get-Content .\transcript.txt -Raw
./scripts/createThumbnail.ps1 -key $key -video $distVideo -transcript $transcript

$title = Get-Content .\title.txt -Raw
$tags = Get-Content .\tags.txt -Raw
$description = Get-Content .\description.txt -Raw

dotnet run video animate $distVideo

$videoId = $(dotnet run -- upload video $animatedVideo `
        -v `
        -t $title `
        -r $releaseDateTime `
        -a $tags `
        -d $description `
        -i $clientId `
        -s $clientSecret)

Write-Host "Video ID: $videoId"

dotnet run -- upload thumbnail $thumbnail `
    -v $videoId `
    -i $clientId `
    -s $clientSecret

