using System.Text.Json;
using System.Text.Json.Serialization;

namespace IldVann.Importer;

// See: https://gaevoy.com/2023/09/26/dotnet-serialization-unknown-enums-handling-api.html
public class GenericUnknownEnumConverter<T> : JsonConverter<T> where T : struct, Enum
{
    private readonly JsonConverter<T> _underlying;

    public GenericUnknownEnumConverter(JsonConverter<T> underlying)
        => _underlying = underlying;

    public override T Read(ref Utf8JsonReader reader, Type enumType, JsonSerializerOptions options)
    {
        try
        {
            return _underlying.Read(ref reader, enumType, options);
        }
        catch (JsonException) when (enumType.IsEnum)
        {
            return default;
        }
    }

    public override bool CanConvert(Type typeToConvert)
        => _underlying.CanConvert(typeToConvert);

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        => _underlying.Write(writer, value, options);

    public override T ReadAsPropertyName(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => _underlying.ReadAsPropertyName(ref reader, typeToConvert, options);

    public override void WriteAsPropertyName(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        => _underlying.WriteAsPropertyName(writer, value, options);
}

public class UnknownEnumConverter : JsonConverterFactory
{
    private readonly JsonStringEnumConverter _underlying = new();

    public sealed override bool CanConvert(Type enumType)
        => _underlying.CanConvert(enumType);

    public sealed override JsonConverter CreateConverter(Type enumType, JsonSerializerOptions options)
    {
        var underlyingConverter = _underlying.CreateConverter(enumType, options);
        var converterType = typeof(GenericUnknownEnumConverter<>).MakeGenericType(enumType);
        return (JsonConverter)Activator.CreateInstance(converterType, underlyingConverter)!;
    }
}
