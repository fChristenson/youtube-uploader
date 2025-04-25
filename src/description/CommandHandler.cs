using System.CommandLine;
using Shared;

namespace Description;

public static class CommandHandler
{
    public static void AddCommand(RootCommand cmd)
    {
        // description command
        var descriptionCommand = CommandUtils.GetTranscriptCommand("description");

        var arg = CommandUtils.GetTranscriptArgument();
        descriptionCommand.AddArgument(arg);

        var apiKey = CommandUtils.GetApiKeyOption();
        descriptionCommand.AddOption(apiKey);

        descriptionCommand.SetHandler(DescriptionExtractor.GetDescriptionAsync, arg, apiKey);

        cmd.Add(descriptionCommand);

    }
}