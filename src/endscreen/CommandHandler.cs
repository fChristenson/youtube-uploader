using System.CommandLine;

namespace EndScreen;

public static class CommandHandler
{
    public static void AddCommand(RootCommand cmd)
    {
        var endScreenCommand = new Command("endscreen", "Configure end screen for video. Close Chrome before using.");

        var arg = new Argument<string[]>("list of video ID's");
        endScreenCommand.AddArgument(arg);

        var chromeUserData = new Option<string>("--userdata", "Path to Chrome user data. C:\\Users\\<username>\\AppData\\Local\\Google\\Chrome\\User Data");
        chromeUserData.IsRequired = true;
        chromeUserData.AddAlias("-u");
        endScreenCommand.Add(chromeUserData);

        endScreenCommand.SetHandler(EndScreenConfigurator.ConfigureEndScreen, chromeUserData, arg);

        cmd.Add(endScreenCommand);
    }
}