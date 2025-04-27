using Shared;

namespace Description;

public static class DescriptionExtractor
{

    public static async Task GetDescriptionAsync(string transcript, string apiKey)
    {
        string prompt = $"""
        Based on the following transcript, write a YouTube video description focused on programming, web development, and software engineering topics.
        The description should:
        - Summarize the main idea clearly in 2 to 5 sentences
        - Be engaging, informative, and relevant to developers and tech enthusiasts
        - Naturally include technical keywords related to the content
        - Encourage viewers to comment, ask questions, or share their opinions
        
        Transcript:
        "{transcript}"
        """;

        var systemRole = "You are a content creator helping users craft great YouTube video descriptions.";
        var response = await GptApiClient.SendPrompt(prompt, systemRole, apiKey);
        Console.WriteLine(response);
    }
}
