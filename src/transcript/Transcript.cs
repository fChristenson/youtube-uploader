using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Transcript;

public static class Transcript
{
    public static async Task GetTranscriptAsync(string filePath, string apiKey)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("❌ File does not exist.");
            return;
        }

        using var httpClient = new HttpClient();
        using var form = new MultipartFormDataContent();

        var fileStream = File.OpenRead(filePath);
        var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");

        form.Add(streamContent, "file", Path.GetFileName(filePath));
        form.Add(new StringContent("whisper-1"), "model");

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var response = await httpClient.PostAsync("https://api.openai.com/v1/audio/transcriptions", form);
        var responseText = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var json = JsonConvert.DeserializeObject<IDictionary<string, object>>(responseText);
            Console.WriteLine(json!["text"]);
        }
        else
        {
            throw new Exception(responseText);
        }
    }
}