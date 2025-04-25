using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Shared;

namespace VideoManager.Lister;

public static class VideoLister
{
    [Obsolete]
    public static async Task ListVideosByDateRangeAsync(DateTime startDate, DateTime endDate, string clientId, string clientSecret, string channelId)
    {
        // Initialize the OAuth 2.0 authentication process
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
        // Initialize YouTube service
        var youtubeService = new YouTubeService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "YouTubeVideoLister"
        });

        // Define the date range
        string publishedAfter = startDate.ToString("yyyy-MM-ddTHH:mm:ssZ");
        string publishedBefore = endDate.ToString("yyyy-MM-ddTHH:mm:ssZ");

        var videoIds = await GetVideosByDateRangeAsync(youtubeService, channelId, publishedAfter, publishedBefore);

        Console.WriteLine($"Videos uploaded between {startDate.ToShortDateString()} and {endDate.ToShortDateString()}:");
        Console.WriteLine(string.Join(",", videoIds));
    }

    // Method to get video IDs uploaded within a date range
    [Obsolete]
    private static async Task<List<string>> GetVideosByDateRangeAsync(YouTubeService youtubeService, string channelId, string publishedAfter, string publishedBefore)
    {
        var videoIds = new List<string>();
        string? nextPageToken = null;

        do
        {
            // Make the API request to get videos uploaded in the specified date range
            var searchRequest = youtubeService.Search.List("snippet");
            searchRequest.ChannelId = channelId;
            searchRequest.PublishedAfter = publishedAfter;
            searchRequest.PublishedBefore = publishedBefore;
            searchRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
            searchRequest.MaxResults = 50;
            searchRequest.PageToken = nextPageToken;

            var searchResponse = await searchRequest.ExecuteAsync();

            // Process the search results and extract video IDs
            foreach (var item in searchResponse.Items)
            {
                if (item.Id.Kind == "youtube#video")
                {
                    videoIds.Add(item.Id.VideoId);
                }
            }

            nextPageToken = searchResponse.NextPageToken;

        } while (nextPageToken != null); // Loop until all pages are processed

        return videoIds;
    }
}