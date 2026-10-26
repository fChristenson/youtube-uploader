using Shared;

namespace Slice;

public static class Slice
{
    public static void SliceFile(string filePath, int seconds)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("❌ File not found.");
            return;
        }

        if (seconds <= 0)
        {
            Console.WriteLine("❌ Invalid duration. Provide a number of seconds > 0.");
            return;
        }

        string fileName = Path.GetFileName(filePath);
        string directory = Path.GetDirectoryName(filePath) ?? ".";
        string slicePath = Path.Combine(directory, $"slice.{fileName}");

        FFmpeg.Run($"-y -i \"{filePath}\" -t {seconds} -c copy \"{slicePath}\"");

        if (File.Exists(slicePath))
        {
            Console.WriteLine($"✅ Created: {slicePath} (first {seconds}s)");
        }
        else
        {
            Console.WriteLine("❌ Failed to slice video.");
        }
    }
}