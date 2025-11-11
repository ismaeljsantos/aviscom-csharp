using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aviscom.Utils.JsonConverters
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        // Os formatos que queremos aceitar
        private readonly string[] _formats = { "ddMMyyyy", "dd/MM/yyyy", "dd-MM-yyyy" };

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException("Esperava-se uma string para a conversão da data.");
            }

            string? dateString = reader.GetString();
            if (string.IsNullOrEmpty(dateString))
            {
                throw new JsonException("A string da data não pode ser nula ou vazia.");
            }

            // Tenta fazer o "parse" usando os formatos exatos
            if (DateTime.TryParseExact(dateString,
                                     _formats,
                                     CultureInfo.InvariantCulture,
                                     DateTimeStyles.None,
                                     out DateTime result))
            {
                return result;
            }

            // (Opcional) Tenta também o formato padrão ISO 8601 (ex: "1990-04-01T00:00:00")
            if (DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out result))
            {
                return result.ToUniversalTime();
            }

            // Se falhar em todos, lança um erro
            throw new JsonException($"A data '{dateString}' não está em um formato reconhecido (ddMMyyyy, dd/MM/yyyy, ou dd-MM-yyyy).");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Ao "escrever" (serializar) a data de volta para o JSON,
            // usamos o formato padrão ISO 8601 (recomendado).
            writer.WriteStringValue(value.ToString("o", CultureInfo.InvariantCulture));
        }
    }
}