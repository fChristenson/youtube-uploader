using System.CommandLine;
using Shared;

namespace Upload.VlogVideo;

public static class CommandHandler
{
    public static void AddCommand(Command cmd)
    {
        var uploadCommand = new Command("video", "Upload a video to youtube channel");

        var clientId = CommandUtils.GetGCPClientIdOption();
        uploadCommand.AddOption(clientId);

        var clientSecret = CommandUtils.GetGCPClientSecretOption();
        uploadCommand.AddOption(clientSecret);

        var title = new Option<string>("--title", "String with Youtube video title");
        title.AddAlias("-t");
        title.IsRequired = true;
        uploadCommand.AddOption(title);

        var description = new Option<string>("--description", "String with Youtube video description");
        description.AddAlias("-d");
        uploadCommand.AddOption(description);

        var tags = new Option<string[]>("--tag", "String with Youtube video tags");
        tags.AddAlias("-a");
        uploadCommand.AddOption(tags);

        var releaseDate = new Option<string>("--release", "String with date for Youtube video release");
        releaseDate.AddAlias("-r");
        uploadCommand.AddOption(releaseDate);

        var categoryId = new Option<string>("--categoryId", getDefaultValue: () => "22", "String with Youtube video categoryId");
        categoryId.AddAlias("-c");
        uploadCommand.AddOption(categoryId);

        var silent = new Option<bool>("--videoId", getDefaultValue: () => false, "Only logs the videoId once the upload is done.");
        silent.AddAlias("-v");
        uploadCommand.AddOption(silent);

        var filePath = new Argument<string>("filepath to video");
        uploadCommand.AddArgument(filePath);

        uploadCommand.SetHandler(async (context) =>
        {
            var clientIdValue = context.ParseResult.GetValueForOption(clientId)!;
            var clientSecretValue = context.ParseResult.GetValueForOption(clientSecret)!;
            var fileValue = context.ParseResult.GetValueForArgument(filePath)!;
            var titleValue = context.ParseResult.GetValueForOption(title)!;
            var descriptionValue = context.ParseResult.GetValueForOption(description)!;
            var tagsValue = context.ParseResult.GetValueForOption(tags)!;
            var categoryIdValue = context.ParseResult.GetValueForOption(categoryId)!;
            var releaseDateValue = context.ParseResult.GetValueForOption(releaseDate)!;
            var silentValue = context.ParseResult.GetValueForOption(silent)!;

            Logger.SilentMode = silentValue;

            await VideoUploader.UploadVideoAsync(
                clientIdValue,
                clientSecretValue,
                fileValue,
                titleValue,
                descriptionValue,
                tagsValue,
                categoryIdValue,
                releaseDateValue
            );
        });
        cmd.AddCommand(uploadCommand);
    }
}
