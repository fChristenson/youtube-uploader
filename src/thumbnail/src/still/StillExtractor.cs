using Shared;

namespace Thumbnail.Still;

public static class StillExtractor
{
    public static void GetStillImage(string videoPath, int offset)
    {
        if (!File.Exists(videoPath))
        {
            Console.WriteLine("Video file not found!");
            return;
        }

        string outputImagePath = $"still.{Path.GetFileName(videoPath)}.jpg";

        // Format seconds as HH:mm:ss
        TimeSpan time = TimeSpan.FromSeconds(offset);
        string formattedTime = time.ToString(@"hh\:mm\:ss");

        // FFmpeg arguments
        string arguments = $"-y -i \"{videoPath}\" -ss {formattedTime} -vframes 1 -q:v 2 \"{outputImagePath}\"";

        FFmpeg.Run(arguments);
        Console.WriteLine(File.Exists(outputImagePath)
            ? $"Still image saved to: {outputImagePath}"
            : "Failed to create still image.");
    }
}