using System.CommandLine;

namespace Thumbnail.Still;

public static class CommandHandler
{
    public static void AddCommand(Command cmd)
    {
        var stillCommand = new Command("still", "Get a still image from video");

        var arg = new Argument<string>("path to file", "Path to video file to get still image from");
        stillCommand.AddArgument(arg);

        var offset = new Option<int>("--offset", getDefaultValue: () => 10, "The number of seconds into the video to get the still.");
        offset.AddAlias("-o");
        stillCommand.AddOption(offset);

        stillCommand.SetHandler(StillExtractor.GetStillImage, arg, offset);

        cmd.Add(stillCommand);
    }
}
