using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Converters
{
    public class ArrayToListConverter<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) return;
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object value;
            switch (reader.TokenType)
            {
                case JsonToken.StartObject:
                    T instance = (T) serializer.Deserialize(reader, typeof(T));
                    value = new List<T> {instance};
                    break;
                case JsonToken.StartArray:
                    value = serializer.Deserialize<List<T>>(reader);
                    break;
                default:
                    value = null;
                    break;
            }
            return value;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T[]);
        }
    }
}