﻿using System;
using System.Collections.Generic;

namespace CoffeeJelly.TelegramBotApiWrapper.Extensions
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