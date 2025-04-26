using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;

namespace VideoManager.Lister;

public static class VideoLister
{
    [Obsolete]
    public static async Task ListVideosByDateRangeAsync(DateTime startDate, DateTime endDate, string clientId, string clientSecret)
    {
        var clientSecrets = new ClientSecrets
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        };

        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            clientSecrets,
            [YouTubeService.Scope.YoutubeReadonly],
            "user",
            CancellationToken.None,
            new FileDataStore("YouTubeLister")
        );

        var youtubeService = new YouTubeService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "YouTubeVideoLister"
        });

        // Step 1: Get the uploads playlist ID for the authenticated user's channel
        var channelsRequest = youtubeService.Channels.List("contentDetails");
        channelsRequest.Mine = true;
        var channelsResponse = await channelsRequest.ExecuteAsync();

        var uploadsPlaylistId = channelsResponse.Items[0].ContentDetails.RelatedPlaylists.Uploads;

        // Step 2: Gather all video IDs from the uploads playlist
        var allVideoIds = new List<string>();
        string? nextPageToken = null;

        do
        {
            var playlistRequest = youtubeService.PlaylistItems.List("contentDetails");
            playlistRequest.PlaylistId = uploadsPlaylistId;
            playlistRequest.MaxResults = 50;
            playlistRequest.PageToken = nextPageToken;

            var playlistResponse = await playlistRequest.ExecuteAsync();

            allVideoIds.AddRange(playlistResponse.Items.Select(i => i.ContentDetails.VideoId));

            nextPageToken = playlistResponse.NextPageToken;
        } while (nextPageToken != null);

        // Step 3: Batch fetch video details and filter by publish time
        var filteredVideoIds = new List<string>();
        for (int i = 0; i < allVideoIds.Count; i += 50)
        {
            var batch = allVideoIds.Skip(i).Take(50).ToList();
            var videosRequest = youtubeService.Videos.List("snippet,status");
            videosRequest.Id = string.Join(",", batch);
            var videosResponse = await videosRequest.ExecuteAsync();

            foreach (var video in videosResponse.Items)
            {
                var publishedAt = video.Snippet.PublishedAt;
                if (publishedAt.HasValue &&
                    publishedAt.Value >= startDate.ToUniversalTime() &&
                    publishedAt.Value <= endDate.ToUniversalTime())
                {
                    filteredVideoIds.Add(video.Id);
                }
            }
        }

        Console.WriteLine($"Videos uploaded between {startDate} and {endDate}:");
        Console.WriteLine(string.Join(", ", filteredVideoIds));
    }
}