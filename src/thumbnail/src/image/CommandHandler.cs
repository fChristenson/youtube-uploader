using System.CommandLine;

namespace Thumbnail.Image;

public static class CommandHandler
{
    public static void AddCommand(Command cmd)
    {
        var imageCommand = new Command("image", "Create thumbnail image");
        var defaultImage = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src/thumbnail/src/image", "background.png");
        var arg = new Argument<string>("path to file", getDefaultValue: () => defaultImage, "Path to image file to use as background for thumbnail");
        imageCommand.AddArgument(arg);

        var still = new Option<string>("--still", "Path to file to use as still image on the thumbnail");
        still.AddAlias("-s");
        still.IsRequired = true;
        imageCommand.AddOption(still);

        var text = new Option<string>("--text", getDefaultValue: () => "Test", "Text to add to the thumbnail image");
        text.AddAlias("-t");
        imageCommand.AddOption(text);

#pragma warning disable CS0612 // Type or member is obsolete
        imageCommand.SetHandler(ImageCreator.AddTextToImage, arg, text, still);
#pragma warning restore CS0612 // Type or member is obsolete

        cmd.Add(imageCommand);
    }
}