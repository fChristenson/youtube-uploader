using System.CommandLine;

namespace VideoManager.Animator;

public static class CommandHandler
{
    public static void AddCommand(Command cmd)
    {
        var animateCommand = new Command("animate", "Add like and subscribe animation to video");

        var arg = new Argument<string>("path to video", "Path to video file you want to process");
        animateCommand.AddArgument(arg);

        var startTime = new Option<int>("from", getDefaultValue: () => 5, "Number of seconds in to the video to start the animation");
        animateCommand.Add(startTime);

        var endTime = new Option<int>("to", getDefaultValue: () => 15, "Number of seconds in to the video to stop the animation");
        animateCommand.Add(endTime);

        animateCommand.SetHandler(Animator.AddLikeAndSubscribeAnimation, arg, startTime, endTime);

        cmd.Add(animateCommand);
    }
}