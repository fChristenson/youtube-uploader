using System.CommandLine;
using Shared;

namespace Thumbnail.Text;

public static class CommandHandler
{
    public static void AddCommand(Command cmd)
    {
        var textCommand = new Command("text", "Get thumbnail image text.");

        var arg = CommandUtils.GetTranscriptArgument();
        textCommand.AddArgument(arg);

        var apiKey = CommandUtils.GetApiKeyOption();
        textCommand.AddOption(apiKey);

        textCommand.SetHandler(TextExtractor.GetThumbnailTextAsync, arg, apiKey);

        cmd.Add(textCommand);
    }
}
