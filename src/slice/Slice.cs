namespace Slice;

public static class Slice
{
    public static void SliceFile(string filePath, int size)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("❌ File not found.");
            return;
        }

        if (size <= 0)
        {
            Console.WriteLine("❌ Invalid chunk size. Provide a number > 0.");
            return;
        }

        long chunkSize = size * 1024L * 1024L;
        byte[] buffer = new byte[chunkSize];
        string fileName = Path.GetFileName(filePath);
        string directory = Path.GetDirectoryName(filePath) ?? ".";

        using FileStream inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var ext = Path.GetExtension(filePath);

        string slicePath = Path.Combine(directory, $"slice.{fileName}");
        using FileStream outputStream = new FileStream(slicePath, FileMode.Create, FileAccess.Write);

        int bytesRead = inputStream.Read(buffer, 0, buffer.Length);
        outputStream.Write(buffer, 0, bytesRead);

        Console.WriteLine($"✅ Created: {slicePath} ({bytesRead} bytes)");
    }
}