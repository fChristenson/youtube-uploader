using System.Diagnostics;

namespace Shared;

public static class FFmpeg
{
    static Process? ffmpegProcess = null;

    public static void Run(string arguments)
    {
        var processInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Console.CancelKeyPress += OnCancelKeyPress;
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;

        try
        {
            ffmpegProcess = new Process
            {
                StartInfo = processInfo,
                EnableRaisingEvents = true
            };

            ffmpegProcess.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                    Console.WriteLine(args.Data);
            };

            ffmpegProcess.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                    Console.WriteLine(args.Data);
            };

            ffmpegProcess.Start();

            ffmpegProcess.BeginOutputReadLine();
            ffmpegProcess.BeginErrorReadLine();

            ffmpegProcess.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error running FFmpeg: " + ex.Message);
        }
    }


    private static void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        KillFFmpeg();
        e.Cancel = true; // Prevents immediate termination
    }

    private static void OnProcessExit(object? sender, EventArgs e)
    {
        KillFFmpeg();
    }

    private static void KillFFmpeg()
    {
        if (ffmpegProcess != null && !ffmpegProcess.HasExited)
        {
            try
            {
                ffmpegProcess.Kill(true);
                Console.WriteLine("\nFFmpeg process was terminated.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error terminating FFmpeg: " + ex.Message);
            }
        }
    }
}