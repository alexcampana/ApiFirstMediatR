using System.Reflection;

namespace ApiFirstMediatR.Generator.Utils;

internal static class EmbeddedResource
{
    public static string GetContent(string relativePath)
    {
        var baseName = Assembly.GetExecutingAssembly().GetName().Name;
        var resourceName = relativePath
            .TrimStart('.')
            .Replace(Path.DirectorySeparatorChar, '.')
            .Replace(Path.AltDirectorySeparatorChar, '.');

        using var stream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream($"{baseName}.{resourceName}");

        if (stream == null)
            throw new FileNotFoundException($"File {baseName}.{resourceName} was not found in assembly");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}