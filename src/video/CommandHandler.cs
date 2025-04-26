using System.CommandLine;

namespace VideoManager;

public static class CommandHandler
{
    public static void AddCommand(RootCommand cmd)
    {
        // video command
        var videoCommand = new Command("video", "Manage videos");

        Optimizer.CommandHandler.AddCommand(videoCommand);
        Lister.CommandHandler.AddCommand(videoCommand);
        Animator.CommandHandler.AddCommand(videoCommand);

        cmd.Add(videoCommand);
    }
}