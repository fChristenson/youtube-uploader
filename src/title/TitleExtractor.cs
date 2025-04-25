using System.Text.Json;
using Shared;

namespace Title;

public static class TitleExtractor
{

    public static async Task GetTitleAsync(string transcript, string apiKey)
    {
        string prompt = $"""
        Based on the following transcript, generate a compelling YouTube video title. Keep it under 100 characters, click-worthy, and relevant to the main idea.

        Transcript:
        "{transcript}"
        """;
        var systemRole = "You are a YouTube video strategist helping creators craft great titles.";
        var response = await GptApiClient.SendPrompt(prompt, systemRole, apiKey);
        Console.WriteLine(JsonSerializer.Deserialize<string>(response));
    }
}
