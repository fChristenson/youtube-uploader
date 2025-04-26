using Shared;

namespace VideoManager.Animator;

public static class Animator
{
    public static void AddLikeAndSubscribeAnimation(string video, int startTime, int endTime)
    {
        var animationVideo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src/video/src/animator", "animation.mp4");

        string arguments = $"-y -i \"{video}\" -itsoffset {startTime} -i \"{animationVideo}\" -filter_complex " +
            "\"[1:v]chromakey=0x00FF00:0.1:0.2,scale=iw*0.15:ih*0.15:flags=lanczos,format=yuva420p[anim];" +
            $"[0:v][anim]overlay=60:(main_h-overlay_h-0):enable='between(t,{startTime},{endTime})'[out]\" " +
            $"-map \"[out]\" -map 0:a -c:v libx264 -preset fast -crf 22 -c:a copy \"animated.{video}\"";

        FFmpeg.Run(arguments);
    }
}
