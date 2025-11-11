using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Aviscom.Utils.JsonConverters
{
    // Este conversor formata um DateTime como "dd/MM/yyyy" ao escrever o JSON
    public class DateOnlyJsonConverter : JsonConverter<DateTime>
    {
        private const string Format = "dd/MM/yyyy";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Este conversor é focado em formatar a *saída* (Write).
            // A *entrada* (Read) já é tratada pelo 'CustomDateTimeConverter'
            // no DTO de Request.
            throw new NotImplementedException("Este conversor é apenas para serialização (saída).");
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Escreve a data no formato dd/MM/yyyy
            writer.WriteStringValue(value.ToString(Format, CultureInfo.InvariantCulture));
        }
    }
}