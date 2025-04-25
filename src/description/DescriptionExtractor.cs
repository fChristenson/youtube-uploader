using Shared;

namespace Description;

public static class DescriptionExtractor
{

    public static async Task GetDescriptionAsync(string transcript, string apiKey)
    {
        string prompt = $"""
        Based on the following transcript, write a YouTube video description.
        The description should:
        - Summarize the main idea in 2 to 5 sentences
        - Be clear, engaging, and informative
        - Include relevant keywords naturally
        - Encourage viewers to comment or share their opinion

        Transcript:
        "{transcript}"
        """;
        var systemRole = "You are a content creator helping users craft great YouTube video descriptions.";
        var response = await GptApiClient.SendPrompt(prompt, systemRole, apiKey);
        Console.WriteLine(response);
    }
}
