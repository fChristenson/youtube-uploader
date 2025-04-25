namespace Shared;

public static class Logger
{
    public static bool SilentMode = false;

    public static void Log(object? args)
    {
        if (SilentMode) return;
        Console.WriteLine(args);
    }

    public static void SilentModeLog(object? args)
    {
        Console.WriteLine(args);
    }
}