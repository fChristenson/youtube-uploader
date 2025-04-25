using System.Text;
using System.Text.RegularExpressions;
using SkiaSharp;

namespace Thumbnail.Image;

public static class ImageCreator
{
    [Obsolete]
    public static void AddTextToImage(string inputPath, string text, string stillImagePath)
    {
        if (!File.Exists(inputPath))
            throw new ArgumentException($"Could not find background file {inputPath}");

        if (!File.Exists(stillImagePath))
            throw new ArgumentException($"Could not find still image file {stillImagePath}");

        var outputPath = $"thumbnail.{Path.GetFileName(stillImagePath)}";
        var fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src/thumbnail/src/image", "Oswald-Bold.ttf");

        using var backgroundBitmap = SKBitmap.Decode(inputPath);
        using var stillBitmap = SKBitmap.Decode(stillImagePath);
        using var surface = SKSurface.Create(new SKImageInfo(backgroundBitmap.Width, backgroundBitmap.Height));
        var canvas = surface.Canvas;

        // Draw background
        canvas.Clear(SKColors.Black);
        canvas.DrawBitmap(backgroundBitmap, 0, 0);

        // Resize and draw the still image with adjustments
        float targetWidth = backgroundBitmap.Width * 0.4f; // slightly smaller
        float scale = targetWidth / stillBitmap.Width;
        float targetHeight = stillBitmap.Height * scale;

        // Lowered vertical position
        float x = (backgroundBitmap.Width - targetWidth) / 2f;
        float y = backgroundBitmap.Height * 0.25f;

        var destRect = new SKRect(x, y, x + targetWidth, y + targetHeight);
        float cornerRadius = 20f;

        var clipPath = new SKPath();
        clipPath.AddRoundRect(destRect, cornerRadius, cornerRadius);

        float centerX = destRect.MidX;
        float centerY = destRect.MidY;

        // Save the canvas state
        canvas.Save();

        // Rotate the canvas
        canvas.RotateDegrees(-15, centerX, centerY);

        // Shadow
        var shadowPaint = new SKPaint
        {
            Color = SKColors.Black.WithAlpha(80),
            ImageFilter = SKImageFilter.CreateBlur(8, 8)
        };

        var shadowOffset = new SKPoint(80, 80);
        var shadowRect = new SKRect(
            destRect.Left + shadowOffset.X,
            destRect.Top + shadowOffset.Y,
            destRect.Right + shadowOffset.X,
            destRect.Bottom + shadowOffset.Y
        );

        // Draw shadow
        canvas.Save();
        canvas.ClipRect(new SKRect(0, 0, backgroundBitmap.Width, backgroundBitmap.Height), SKClipOperation.Intersect);
        canvas.DrawBitmap(stillBitmap, shadowRect, shadowPaint);
        canvas.Restore();

        // Clip + draw still image with opacity and tint
        canvas.Save();
        canvas.ClipPath(clipPath, SKClipOperation.Intersect, true);

        // Apply a tint overlay with some transparency (teal-ish color)
        var stillPaint = new SKPaint
        {
            Color = new SKColor(0, 128, 128, 150), // Teal-ish with 30% opacity
            BlendMode = SKBlendMode.Multiply, // Softer blend than SrcOver
            IsAntialias = true
        };

        // Draw the still image first
        canvas.DrawBitmap(stillBitmap, destRect);

        // Then apply the tint overlay (this applies a colored transparency effect)
        canvas.DrawRect(destRect, stillPaint);
        canvas.Restore();

        // Glow effect (behind the white border)
        var glowPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.White.WithAlpha(150),
            StrokeWidth = 5, // Slightly thicker for glow
            ImageFilter = SKImageFilter.CreateBlur(8, 8),
            IsAntialias = true
        };
        canvas.DrawRoundRect(destRect, cornerRadius, cornerRadius, glowPaint);

        // White border (on top of glow)
        var borderPaint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.White,
            StrokeWidth = 4,
            IsAntialias = true
        };
        canvas.DrawRoundRect(destRect, cornerRadius, cornerRadius, borderPaint);

        // Restore canvas rotation
        canvas.Restore();

        // Draw text on top
        using var fontStream = File.OpenRead(fontPath);
        var typeface = SKTypeface.FromStream(fontStream);

        // 1) Figure out wrapping & best fontSize (unchanged)
        float textTargetWidth = backgroundBitmap.Width * 0.9f;
        float maxFontSize = backgroundBitmap.Height / 6f;
        float minFontSize = 12f;
        float fontSize = maxFontSize;

        List<List<(string Word, string Style)>> wrappedLines = null!;
        SKPaint basePaint = null!;

        for (; fontSize > minFontSize; fontSize -= 2)
        {
            basePaint = new SKPaint
            {
                Typeface = typeface,
                TextSize = fontSize,
                IsAntialias = true,
                Style = SKPaintStyle.Fill
            };

            var segments = ParseMarkdownStyledWords(text);
            wrappedLines = WrapStyledText(segments, basePaint, textTargetWidth);

            // measure widest line
            float widest = wrappedLines
                .Select(line => line.Sum(ws =>
                {
                    var p = basePaint.Clone();
                    ApplyStyle(p, ws.Style);
                    return p.MeasureText(ws.Word + " ");
                }))
                .Max();

            if (widest <= textTargetWidth)
                break;
        }

        // 2) Compute vertical start
        float lineHeight = basePaint.TextSize * 1.2f;
        float blockHeight = wrappedLines.Count * lineHeight;
        float textY = backgroundBitmap.Height - blockHeight - 40;

        // 3) Draw each line, centered
        foreach (var line in wrappedLines)
        {
            // measure this line’s total width
            float lineWidth = line.Sum(ws =>
            {
                var p = basePaint.Clone();
                ApplyStyle(p, ws.Style);
                return p.MeasureText(ws.Word + " ");
            });

            // left-edge X so the line is centered
            float cursorX = (backgroundBitmap.Width - lineWidth) / 2f;

            foreach (var (word, style) in line)
            {
                // clone + style paint
                var paint = basePaint.Clone();
                ApplyStyle(paint, style);

                // shadow
                var shadow = paint.Clone();
                shadow.Color = SKColors.Black.WithAlpha(128);

                // draw shadow then text
                canvas.DrawText(word, cursorX + 2, textY + 2, shadow);
                canvas.DrawText(word, cursorX, textY, paint);

                // advance by word width + space
                cursorX += paint.MeasureText(word + " ");
            }

            textY += lineHeight;
        }

        using var img = surface.Snapshot();
        using var data = img.Encode(SKEncodedImageFormat.Jpeg, 90);
        using var outS = File.OpenWrite(outputPath);
        data.SaveTo(outS);

        Console.WriteLine($"✅ Output saved to {outputPath}");
    }

    private static List<(string Word, string Style)> ParseMarkdownStyledWords(string input)
    {
        var segments = new List<(string, string)>();

        // Regular expression to match bold (**word**) or italic (*word*) emphasis
        var regex = new Regex(@"(\*\*[^*]+\*\*|\*[^*]+\*)|([^*\s]+)");

        foreach (Match match in regex.Matches(input))
        {
            string word = match.Value;

            // Check if the match is bold
            if (word.StartsWith("**") && word.EndsWith("**") && word.Length > 4)
            {
                segments.Add((word.Substring(2, word.Length - 4), "bold"));
            }
            // Check if the match is italic
            else if (word.StartsWith("*") && word.EndsWith("*") && word.Length > 2)
            {
                segments.Add((word.Substring(1, word.Length - 2), "italic"));
            }
            // Otherwise, treat it as a normal word
            else
            {
                segments.Add((word, "normal"));
            }
        }

        return segments;
    }

    [Obsolete]
    private static void ApplyStyle(SKPaint paint, string style)
    {
        switch (style)
        {
            case "bold":
                paint.FakeBoldText = true;
                paint.Color = SKColors.Orange;
                break;
            case "italic":
                paint.TextSkewX = -0.25f;
                paint.Color = SKColors.LightGreen;
                break;
            default:
                paint.Color = SKColors.LightCyan;
                break;
        }
    }

    [Obsolete]
    private static List<List<(string Word, string Style)>> WrapStyledText(
        List<(string Word, string Style)> words, SKPaint paint, float maxWidth)
    {
        var lines = new List<List<(string, string)>>();
        var currentLine = new List<(string, string)>();
        float currentWidth = 0;

        foreach (var (word, style) in words)
        {
            var tempPaint = paint.Clone();
            ApplyStyle(tempPaint, style);
            var width = tempPaint.MeasureText(word + " ");

            if (currentWidth + width > maxWidth && currentLine.Any())
            {
                lines.Add(currentLine);
                currentLine = new List<(string, string)> { (word, style) };
                currentWidth = width;
            }
            else
            {
                currentLine.Add((word, style));
                currentWidth += width;
            }
        }

        if (currentLine.Any())
            lines.Add(currentLine);

        return lines;
    }

}
