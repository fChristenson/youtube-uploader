using System.Text.Json;
using Shared;

namespace Title;

public static class TitleExtractor
{

    public static async Task GetTitleAsync(string transcript, string apiKey)
    {
        string prompt = $"""
        Based on the following transcript, generate a compelling YouTube video title focused on programming, web development, and software engineering topics.
        
        Guidelines:
        - Keep it under 100 characters
        - Make it highly click-worthy and engaging
        - Reflect the main idea clearly
        - Naturally include relevant tech-related keywords when possible
        - If using emojis, use only common ones like 🚀, 🔥, 👍, 💻, 🎯, 🎉
        - Avoid complex, rare, or platform-specific emojis

        
        Transcript:
        "{transcript}"
        """;

        var systemRole = "You are a YouTube video strategist helping creators craft great titles.";
        var response = await GptApiClient.SendPrompt(prompt, systemRole, apiKey);
        Console.WriteLine(JsonSerializer.Deserialize<string>(response));
    }
}
