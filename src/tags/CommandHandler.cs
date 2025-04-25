using System.CommandLine;
using Shared;

namespace Tags;

public static class CommandHandler
{
    public static void AddCommand(RootCommand cmd)
    {
        // Tags command
        var tagsCommand = CommandUtils.GetTranscriptCommand("tags");

        var arg = CommandUtils.GetTranscriptArgument();
        tagsCommand.AddArgument(arg);

        var apiKey = CommandUtils.GetApiKeyOption();
        tagsCommand.AddOption(apiKey);

        tagsCommand.SetHandler(KeywordExtractor.GetTagsAsync, arg, apiKey);

        cmd.Add(tagsCommand);
    }
}