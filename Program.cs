using System.CommandLine;

var rootCommand = new RootCommand("Youtube Upload CLI");

Description.CommandHandler.AddCommand(rootCommand);
Mp3.CommandHandler.AddCommand(rootCommand);
Tags.CommandHandler.AddCommand(rootCommand);
Thumbnail.CommandHandler.AddCommand(rootCommand);
Title.CommandHandler.AddCommand(rootCommand);
Upload.CommandHandler.AddCommand(rootCommand);
Slice.CommandHandler.AddCommand(rootCommand);
Transcript.CommandHandler.AddCommand(rootCommand);
VideoManager.CommandHandler.AddCommand(rootCommand);
EndScreen.CommandHandler.AddCommand(rootCommand);

return await rootCommand.InvokeAsync(args);
