using Shared;

namespace Tags;

public static class KeywordExtractor
{

    public static async Task GetTagsAsync(string transcript, string apiKey)
    {
        string prompt = $"""
        Extract 10 YouTube-friendly keywords or short phrases from the following text. List them on a single line separated by ",". Avoid filler words and focus on topic-relevant terms:

        "{transcript}"
        """;
        var systemRole = "You are a helpful assistant that extracts relevant YouTube keywords from transcripts.";
        var response = await GptApiClient.SendPrompt(prompt, systemRole, apiKey);
        Console.WriteLine(response);
    }
}
