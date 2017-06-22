using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;

namespace CoffeeJelly.TelegramBotApiWrapper.Helpers
{
    internal class Content
    {
        public void Add(string name, string value)
        {
            name.NullInspect(nameof(name));
            value.NullInspect(nameof(value));

            if (Json)
            {
                JsonData.Add(name, value);
                return;
            }
            Data.Add(new KeyValuePair<string, string>(name, value));
        }

        public List<KeyValuePair<string, string>> Data = new List<KeyValuePair<string, string>>();

        public Dictionary<string, string> JsonData = new Dictionary<string, string>();

        public static implicit operator List<KeyValuePair<string, string>>(Content obj)
        {
            return obj.Data;
        }

        public static implicit operator Dictionary<string, string>(Content obj)
        {
            return obj.JsonData;
        }

        public bool Json { get; set; }
    }
}
