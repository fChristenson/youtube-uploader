using System.CommandLine;
using Shared;

namespace Transcript;

public static class CommandHandler
{
    public static void AddCommand(RootCommand cmd)
    {
        // transcript command
        var transcriptCommand = new Command("transcript", "Get OpenAI Whisper transcript of mp3 file");

        var arg = new Argument<string>("path to file", "Path to mp3 file to get transcript for");
        transcriptCommand.AddArgument(arg);

        var apiKey = CommandUtils.GetApiKeyOption();
        transcriptCommand.AddOption(apiKey);

        transcriptCommand.SetHandler(Transcript.GetTranscriptAsync, arg, apiKey);

        cmd.Add(transcriptCommand);
    }
}