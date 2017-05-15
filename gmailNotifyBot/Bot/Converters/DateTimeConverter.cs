using System;
using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Bot.Converters
{
    internal class DateTimeConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) return;

            var initDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            writer.WriteRawValue(((DateTime)value - initDate).TotalMilliseconds + "000");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(reader.Value));
            return dateTimeOffset.UtcDateTime;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }
    }
}
