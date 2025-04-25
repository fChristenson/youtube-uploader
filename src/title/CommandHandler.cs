using System.CommandLine;
using Shared;

namespace Title;

public static class CommandHandler
{
    public static void AddCommand(RootCommand cmd)
    {
        // title command
        var titleCommand = CommandUtils.GetTranscriptCommand("title");

        var arg = CommandUtils.GetTranscriptArgument();
        titleCommand.AddArgument(arg);

        var apiKey = CommandUtils.GetApiKeyOption();
        titleCommand.AddOption(apiKey);

        titleCommand.SetHandler(TitleExtractor.GetTitleAsync, arg, apiKey);

        cmd.Add(titleCommand);
    }
}