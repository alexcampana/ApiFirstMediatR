namespace ApiFirstMediatR.Generator.Models.Config;

internal sealed class SerializationLibrary
{
    public static readonly SerializationLibrary SystemTextJson = new SerializationLibrary
    {
        Name = "System.Text.Json",
        PropertyNameAttribute = "System.Text.Json.Serialization.JsonPropertyName",
        JsonConverterClass = "System.Text.Json.Serialization.JsonConverter",
        EnumMemberConverterClass = "System.Text.Json.Serialization.JsonStringEnumMemberConverter"
    };

    public static readonly SerializationLibrary NewtonsoftJson = new SerializationLibrary
    {
        Name = "Newtonsoft.Json",
        PropertyNameAttribute = "Newtonsoft.Json.JsonPropertyAttribute",
        JsonConverterClass = "Newtonsoft.Json.JsonConverter",
        EnumMemberConverterClass = "Newtonsoft.Json.Converters.StringEnumConverter"
    };

    private static readonly Dictionary<string, SerializationLibrary> _serializationLibraryDict =
        ImmutableArray.Create(SystemTextJson, NewtonsoftJson)
            .ToDictionary(s => s.Name);

    public static SerializationLibrary? FindSerializationLibrary(string name)
    {
        _serializationLibraryDict.TryGetValue(name, out var value);
        return value;
    }

    public static bool TryGetSerializationLibrary(string name, out SerializationLibrary? value) =>
        _serializationLibraryDict.TryGetValue(name, out value);

    public required string Name { get; set; }
    public required string PropertyNameAttribute { get; set; }
    public required string JsonConverterClass { get; set; }
    public required string EnumMemberConverterClass { get; set; }
}