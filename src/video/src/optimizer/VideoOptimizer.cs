using Shared;

namespace VideoManager.Optimizer;

public static class VideoOptimizer
{
    public static void ProcessVideo(string inputPath)
    {
        if (!File.Exists(inputPath))
        {
            Console.WriteLine("❌ File not found: " + inputPath);
            return;
        }

        string outputPath = $"youtube.{Path.GetFileName(inputPath)}.mp4";

        string ffmpegArgs = $"-y -i \"{inputPath}\" " +
            "-c:v libx264 -preset veryfast -crf 20 -profile:v high -pix_fmt yuv420p " +
            "-c:a aac -b:a 192k -ar 48000 " +
            "-vf \"hqdn3d=1.5:1.5:6:6," +
                   "eq=contrast=1.2:brightness=0.05," +
                   "vignette," +
                   "eq=saturation=1.5\" " +
            "-af \"loudnorm,acompressor=threshold=-18dB:ratio=4:attack=20:release=250\" " +
            "-movflags +faststart " +
            "-threads 0 " +
            $"\"{outputPath}\"";



        Console.WriteLine("🚀 Starting optimization for YouTube...");
        FFmpeg.Run(ffmpegArgs);

        Console.WriteLine("📦 Optimization complete.");
        Console.WriteLine("➡ Output saved to: " + outputPath);
    }
}