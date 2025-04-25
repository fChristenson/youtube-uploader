using System.CommandLine;

namespace Upload;

public static class CommandHandler
{
    public static void AddCommand(RootCommand cmd)
    {
        var uploadCommand = new Command("upload", "Upload a video or thumbnail to youtube channel");
        VlogVideo.CommandHandler.AddCommand(uploadCommand);
        Thumbnail.CommandHandler.AddCommand(uploadCommand);
        cmd.AddCommand(uploadCommand);
    }
}