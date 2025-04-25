using System.CommandLine;

namespace Thumbnail;

public static class CommandHandler
{
    public static void AddCommand(RootCommand cmd)
    {
        // thumbnail command
        var thumbnailCommand = new Command("thumbnail", "Create thumbnail image for video");

        Image.CommandHandler.AddCommand(thumbnailCommand);
        Text.CommandHandler.AddCommand(thumbnailCommand);
        Still.CommandHandler.AddCommand(thumbnailCommand);

        cmd.Add(thumbnailCommand);
    }
}