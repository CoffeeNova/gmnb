using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Extensions
{
    public static class ArgumentsNullInspectorExtension
    {
        public static void NullInspect(this List<KeyValuePair<string, object>> list)
        {
            list.ForEach(i =>
            {
                if (i.Value == null)
                    throw new ArgumentNullException(i.Key);
            });
        }

        public static void NullInspect(this object obj, string name)
        {
            if (obj == null)
                throw new ArgumentNullException(name);
        }
    }
}