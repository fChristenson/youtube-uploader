using System.CommandLine;

namespace Slice;

public static class CommandHandler
{
    public static void AddCommand(RootCommand cmd)
    {
        // slice command
        var sliceCommand = new Command("slice", "Cut the first N seconds of a video");

        var arg = new Argument<string>("path to file");
        sliceCommand.AddArgument(arg);

        var seconds = new Option<int>("--seconds", getDefaultValue: () => 30, "Number of seconds to keep from the start of the video.");
        seconds.AddAlias("-s");
        sliceCommand.AddOption(seconds);

        sliceCommand.SetHandler(Slice.SliceFile, arg, seconds);

        cmd.Add(sliceCommand);
    }
}
