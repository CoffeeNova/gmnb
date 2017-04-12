using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class TelegramActionAttribute : Attribute
    {
        public TelegramActionAttribute(string action)
        {
            Action = action;
        }
        public string Action { get; set; }

        public static string GetActionValue(TelegramMethods.Action action)
        {
            var type = typeof(TelegramMethods.Action);
            var fieldInfoArr = type.GetFields();
            var fieldInfo = fieldInfoArr.First(i => i.Name == action.ToString());
            var attribute = (TelegramActionAttribute)fieldInfo.GetCustomAttribute(typeof(TelegramActionAttribute));

            return attribute.Action;
        }
    }
}