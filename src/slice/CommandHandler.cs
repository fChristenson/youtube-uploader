using System.CommandLine;

namespace Slice;

public static class CommandHandler
{
    public static void AddCommand(RootCommand cmd)
    {
        // slice command
        var sliceCommand = new Command("slice", "Slice file down to a smaller size");

        var arg = new Argument<string>("path to file");
        sliceCommand.AddArgument(arg);

        var size = new Option<int>("--size", getDefaultValue: () => 20, "The size to reduce the file to in MB.");
        size.AddAlias("-s");
        sliceCommand.AddOption(size);

        sliceCommand.SetHandler(Slice.SliceFile, arg, size);

        cmd.Add(sliceCommand);
    }
}
