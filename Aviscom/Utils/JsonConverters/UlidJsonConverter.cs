using System.Text.Json;
using System.Text.Json.Serialization;
using NUlid;

namespace Aviscom.Utils.JsonConverters
{
    public class UlidJsonConverter : JsonConverter<Ulid>
    {
        public override Ulid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String && Ulid.TryParse(reader.GetString(), out var ulid))
            {
                return ulid;
            }

            throw new JsonException("A string não estava em um formato Ulid válido.");
        }

        public override void Write(Utf8JsonWriter writer, Ulid value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}