using System.CommandLine;
using Shared;

namespace VideoManager.Lister;

public static class CommandHandler
{
    public static void AddCommand(Command cmd)
    {
        var listCommand = new Command("list", "List youtube videos uploaded between two DateTime's");

        var from = new Argument<DateTime>("from", "Starting DateTime");
        listCommand.AddArgument(from);

        var to = new Argument<DateTime>("to", "Ending DateTime");
        listCommand.AddArgument(to);

        var clientId = CommandUtils.GetGCPClientIdOption();
        listCommand.AddOption(clientId);

        var clientSecret = CommandUtils.GetGCPClientSecretOption();
        listCommand.AddOption(clientSecret);

        var channelId = new Option<string>("channelId", getDefaultValue: () => "UC0BAd8tPlDqFvDYBemHcQPQ", "Youtube channel ID");
        listCommand.Add(channelId);

#pragma warning disable CS0612 // Type or member is obsolete
        listCommand.SetHandler(VideoLister.ListVideosByDateRangeAsync, from, to, clientId, clientSecret, channelId);
#pragma warning restore CS0612 // Type or member is obsolete

        cmd.Add(listCommand);
    }
}