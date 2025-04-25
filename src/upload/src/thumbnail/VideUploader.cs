using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;

namespace Upload.Thumbnail;

public static class ThumbnailUploader
{
    public static async Task UploadThumbnailAsync(string clientId,
        string clientSecret,
        string thumbnailPath,
        string videoId)
    {
        if (!File.Exists(thumbnailPath))
        {
            Console.WriteLine($"Thumbnail file not found: {thumbnailPath}");
            return;
        }

        var secrets = new ClientSecrets
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };

        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            secrets,
            [YouTubeService.Scope.YoutubeUpload],
            "user",
            CancellationToken.None,
            new FileDataStore("YouTubeUploader")
        );

        var youtubeService = new YouTubeService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "YouTubeThumbnailUploader"
        });

        Console.WriteLine($"Uploading thumbnail for video: {videoId}");

        using var fileStream = new FileStream(thumbnailPath, FileMode.Open, FileAccess.Read);
        var thumbnailSetRequest = youtubeService.Thumbnails.Set(videoId, fileStream, "image/jpeg");

        thumbnailSetRequest.ProgressChanged += progress =>
        {
            if (progress.Status == UploadStatus.Uploading)
                Console.WriteLine($"Uploading: {progress.BytesSent} bytes...");
            else if (progress.Status == UploadStatus.Completed)
                Console.WriteLine("✅ Thumbnail upload completed!");
            else if (progress.Status == UploadStatus.Failed)
                Console.WriteLine($"❌ Upload failed: {progress.Exception}");
        };

        await thumbnailSetRequest.UploadAsync();
    }
}
