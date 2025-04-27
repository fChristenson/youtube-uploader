using Shared;

namespace Thumbnail.Text;

public static class TextExtractor
{

    public static async Task GetThumbnailTextAsync(string transcript, string apiKey)
    {
        string prompt = $"""
        Based on the following transcript, generate a short, punchy phrase (2 to 6 words) for a YouTube thumbnail, focusing on programming, web development, and software engineering topics.
        
        Format:
        - Use **Markdown-style emphasis** on **individual words only**.
        - **Bold** = emotionally charged, surprising, or powerful tech-related words
        - *Italic* = nuanced, ironic, or intriguing terms
        - Do **not** emphasize entire phrases or full sentences.
        - Only emphasize a word if it makes contextual sense.
        - If using emojis, use only common ones like 🚀, 🔥, 👍, 💻, 🎯, 🎉
        - Avoid complex, rare, or platform-specific emojis

        
        Example:
        "**AI** *rewrites* **everything**"
        
        Transcript:
        "{transcript}"
        """;

        var systemRole = "You are an expert in YouTube thumbnail copywriting. Use Markdown for emphasis.";

        var response = await GptApiClient.SendPrompt(prompt, systemRole, apiKey);
        Console.WriteLine(response);
    }
}
