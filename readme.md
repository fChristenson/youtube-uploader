# youtube-uploader

## Getting started

- Install ffmpeg
- Create GCP account
- Create GCP project
- Create GCP OAuth2 credentials
- Set YouTube Data API v3 to enabled for project
- Create OpenAI account
- Create OpenAI api key

```powershell
Description:
  Youtube Upload CLI

Usage:
  YoutubeUploader [command] [options]

Options:
  --version       Show version information
  -?, -h, --help  Show help and usage information

Commands:
  description <text>         Use gpt-3.5-turbo to get description from transcript
  mp3 <path to video file>   Use ffmpeg to convert video to mp3
  tags <text>                Use gpt-3.5-turbo to get tags from transcript
  thumbnail                  Create thumbnail image for video
  title <text>               Use gpt-3.5-turbo to get title from transcript
  upload                     Upload a video or thumbnail to youtube channel
  slice <path to file>       Slice file down to a smaller size
  transcript <path to file>  Get OpenAI Whisper transcript of mp3 file
  video <path to video>      Optimize video for youtube
```