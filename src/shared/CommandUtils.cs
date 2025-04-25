using System.CommandLine;

namespace Shared;

public static class CommandUtils
{
    public static Option<string> GetApiKeyOption()
    {
        var apiKey = new Option<string>("--apiKey", "OpenAI api key. https://platform.openai.com/api-keys");
        apiKey.IsRequired = true;
        apiKey.AddAlias("-k");

        return apiKey;
    }

    public static Argument<string> GetTranscriptArgument() => new Argument<string>("text", "Raw transcript text.");

    public static Command GetTranscriptCommand(string command) => new Command(command, $"Use gpt-3.5-turbo to get {command} from transcript");

    public static Option<string> GetGCPClientIdOption()
    {
        var clientId = new Option<string>("--clientId", "String with GCP clientId");
        clientId.IsRequired = true;
        clientId.AddAlias("-i");
        return clientId;
    }

    public static Option<string> GetGCPClientSecretOption()
    {
        var clientSecret = new Option<string>("--clientSecret", "String with GCP clientSecret");
        clientSecret.IsRequired = true;
        clientSecret.AddAlias("-s");
        return clientSecret;
    }
}