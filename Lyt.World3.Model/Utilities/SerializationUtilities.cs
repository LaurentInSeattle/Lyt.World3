﻿namespace Lyt.World3.Model.Utilities;

public static class SerializationUtilities
{
    public const string ResourcesExtension = ".json";

    private static readonly JsonSerializerOptions jsonSerializerOptions;
    private static readonly Assembly worldAssembly;
    private static string[]? worldResourceNames;

    static SerializationUtilities()
    {
        worldAssembly = Assembly.GetExecutingAssembly();
        jsonSerializerOptions =
            new JsonSerializerOptions
            {
                // 'Classic' properties 
                AllowTrailingCommas = true,
                WriteIndented = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                IgnoreReadOnlyFields = true,
                IgnoreReadOnlyProperties = true,

                // .Net 9 properties 
                AllowOutOfOrderMetadataProperties = true,
                IndentSize = 4,
                RespectRequiredConstructorParameters = true,
                RespectNullableAnnotations = true,
            };
        jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public static string? GetFullResourceName(string name)
    {
        worldResourceNames ??= worldAssembly.GetManifestResourceNames();
        return worldResourceNames.Single(str => str.EndsWith(name));
    }

    public static string LoadEmbeddedTextResource(string name, out string? resourceName)
    {
        resourceName = SerializationUtilities.GetFullResourceName(name);
        if (!string.IsNullOrEmpty(resourceName))
        {
            var stream = worldAssembly.GetManifestResourceStream(resourceName);
            if (stream is not null)
            {
                using (stream)
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        throw new Exception("Failed to load resource: " + name);
    }

    public static byte[] LoadEmbeddedBinaryResource(string name, out string? resourceName)
    {
        resourceName = SerializationUtilities.GetFullResourceName(name);
        if (!string.IsNullOrEmpty(resourceName))
        {
            var stream = worldAssembly.GetManifestResourceStream(resourceName);
            if (stream is not null)
            {
                using (stream)
                {
                    byte[] bytes = new byte[stream.Length];
                    int bytesRead = stream.Read(bytes, 0, bytes.Length);
                    if (bytesRead != bytes.Length)
                    {
                        throw new Exception("Failed to read resource stream: " + name);
                    }

                    return bytes;
                }
            }
        }

        throw new Exception("Failed to load resource: " + name);
    }

    public static string Serialize<T>(T binaryObject) where T : class
    {
        try
        {
            string serialized = JsonSerializer.Serialize(binaryObject, jsonSerializerOptions);
            if (!string.IsNullOrWhiteSpace(serialized))
            {
                return serialized;
            }

            throw new Exception("Serialized as null or empty string.");
        }
        catch (Exception ex)
        {
            string msg = "Failed to serialize " + typeof(T).FullName + "\n" + ex.ToString();
            throw new Exception(msg, ex);
        }
    }

    public static T Deserialize<T>(string serialized) where T : class
    {
        try
        {
            object? deserialized = JsonSerializer.Deserialize<T>(serialized, jsonSerializerOptions);
            if (deserialized is T binaryObject)
            {
                return binaryObject;
            }

            throw new Exception();
        }
        catch (Exception ex)
        {
            string msg = "Failed to deserialize " + typeof(T).FullName + "\n" + ex.ToString();
            throw new Exception(msg, ex);
        }
    }
}
