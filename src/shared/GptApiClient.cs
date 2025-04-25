using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Shared;

public static class GptApiClient
{

    public static async Task<string> SendPrompt(string prompt, string systemRole, string apiKey)
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = systemRole },
                new { role = "user", content = prompt }
            },
            temperature = 0.4
        };

        var response = await httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);

        var resultJson = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            using JsonDocument doc = JsonDocument.Parse(resultJson);
            string output = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString()!;

            return output;
        }
        else
        {
            throw new Exception(resultJson);
        }
    }
}
