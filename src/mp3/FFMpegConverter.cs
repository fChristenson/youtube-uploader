using Shared;

namespace Mp3;

public static class FFMPegConverter
{
    public static void ConvertFile(string filePath)
    {

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Input file does not exist.");
            return;
        }

        string fileName = $"{Path.GetFileName(filePath)}.mp3";
        var arguments = $"-y -i \"{filePath}\" -vn -ar 44100 -ac 2 -ab 192k -f mp3 \"{fileName}\"";
        FFmpeg.Run(arguments);

        if (File.Exists(fileName))
        {
            Console.WriteLine($"✅ MP3 saved to: {fileName}");
        }
        else
        {
            Console.WriteLine("❌ Failed to convert video to MP3.");
        }
    }
}