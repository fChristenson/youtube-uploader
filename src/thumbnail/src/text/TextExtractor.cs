using Shared;

namespace Thumbnail.Text;

public static class TextExtractor
{

    public static async Task GetThumbnailTextAsync(string transcript, string apiKey)
    {
        string prompt = $"""
        Based on the following transcript, generate a short, punchy phrase (2 to 6 words) for a YouTube thumbnail.

        Format:
        - Use **Markdown-style emphasis** on **individual words only**.
        - **Bold** = emotionally charged or surprising words
        - *Italic* = nuanced, ironic, or intriguing words
        - Do **not** emphasize entire phrases or full sentences.
        - Only emphasize a word if it makes contextual sense.

        Example:
        "**AI Coding Tools: Revolutionizing Software Development**" ← too long, too generic, not selective
        "**AI** *rewrites* **everything**"

        Transcript:
        "{transcript}"
        """;


        var systemRole = "You are an expert in YouTube thumbnail copywriting. Use Markdown for emphasis.";

        var response = await GptApiClient.SendPrompt(prompt, systemRole, apiKey);
        Console.WriteLine(response);
    }
}
