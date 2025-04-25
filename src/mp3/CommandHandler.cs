using System.CommandLine;

namespace Mp3;

public static class CommandHandler
{
    public static void AddCommand(RootCommand cmd)
    {
        // mp3 command
        var mp3Command = new Command("mp3", "Use ffmpeg to convert video to mp3");

        var arg = new Argument<string>("path to video file");
        mp3Command.AddArgument(arg);

        mp3Command.SetHandler(FFMPegConverter.ConvertFile, arg);

        cmd.Add(mp3Command);
    }
}