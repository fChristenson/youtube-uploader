using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Shared;

namespace Upload.VlogVideo;

public static class VideoUploader
{
    public static async Task UploadVideoAsync(string clientId,
        string clientSecret,
        string filePath,
        string title,
        string description,
        string[] tags,
        string categoryId,
        string releaseDate)
    {
        Logger.Log($"Starting YouTube upload for: {filePath}");

        var clientSecrets = new ClientSecrets
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };

        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            clientSecrets,
            [YouTubeService.Scope.YoutubeUpload],
            "user",
            CancellationToken.None,
            new FileDataStore("YouTubeUploader")
        );

        var youtubeService = new YouTubeService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "YouTubeUploader"
        });

        var video = new Video
        {
            Snippet = new VideoSnippet
            {
                Title = title,
                Description = description,
                Tags = tags,
                CategoryId = categoryId
            },
            Status = new VideoStatus
            {
                PrivacyStatus = "private",
                PublishAtRaw = releaseDate
            }
        };

        using var fileStream = new FileStream(filePath, FileMode.Open);

        var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");

        videosInsertRequest.ProgressChanged += progress =>
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    Logger.Log($"Uploading: {progress.BytesSent} bytes sent...");
                    break;
                case UploadStatus.Completed:
                    Logger.Log($"✅ Upload completed! Video ID: {videosInsertRequest.ResponseBody.Id}");
                    Logger.SilentModeLog(videosInsertRequest.ResponseBody.Id);
                    break;
                case UploadStatus.Failed:
                    Logger.Log($"❌ Upload failed: {progress.Exception}");
                    break;
            }
        };

        await videosInsertRequest.UploadAsync();
    }
}
