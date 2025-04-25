using System.CommandLine;
using Shared;

namespace VideoOptimizer;

public static class CommandHandler
{
    public static void AddCommand(RootCommand cmd)
    {
        // video command
        var videoCommand = new Command("video", "Optimize video for youtube");

        var arg = new Argument<string>("path to video", "Path to video file you want to process");
        videoCommand.AddArgument(arg);

        videoCommand.SetHandler(VideoOptimizer.ProcessVideo, arg);

        cmd.Add(videoCommand);
    }
}