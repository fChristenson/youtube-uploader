using System.CommandLine;

namespace VideoManager.Optimizer;

public static class CommandHandler
{
    public static void AddCommand(Command cmd)
    {
        // video command
        var videoCommand = new Command("optimize", "Optimize video for youtube");

        var arg = new Argument<string>("path to video", "Path to video file you want to process");
        videoCommand.AddArgument(arg);

        videoCommand.SetHandler(VideoOptimizer.ProcessVideo, arg);

        cmd.Add(videoCommand);
    }
}