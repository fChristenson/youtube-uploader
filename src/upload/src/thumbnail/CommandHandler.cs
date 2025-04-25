using System.CommandLine;
using Shared;

namespace Upload.Thumbnail;

public static class CommandHandler
{
    public static void AddCommand(Command cmd)
    {
        var uploadCommand = new Command("thumbnail", "Upload a thumbnail to youtube channel");

        var clientId = CommandUtils.GetGCPClientIdOption();
        uploadCommand.AddOption(clientId);

        var clientSecret = CommandUtils.GetGCPClientSecretOption();
        uploadCommand.AddOption(clientSecret);

        var videoId = new Option<string>("--videoId", "String with Youtube video id");
        videoId.AddAlias("-v");
        videoId.IsRequired = true;
        uploadCommand.AddOption(videoId);

        var filePath = new Argument<string>("filepath to thumbnail");
        uploadCommand.AddArgument(filePath);

        uploadCommand.SetHandler(ThumbnailUploader.UploadThumbnailAsync,
            clientId,
            clientSecret,
            filePath,
            videoId);

        cmd.AddCommand(uploadCommand);
    }
}