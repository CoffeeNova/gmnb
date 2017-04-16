using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class UpdateAttribute : Attribute
    {
        public UpdateAttribute(string updateType)
        {
            UpdateType = updateType;
        }

        public string UpdateType { get; set; }

        public static string GetUpdateType(UpdateType updateType)
        {
            var type = typeof(UpdateType);
            var fieldInfoArr = type.GetFields();
            var fieldInfo = fieldInfoArr.First(i => i.Name == updateType.ToString());
            var attribute = (UpdateAttribute)fieldInfo.GetCustomAttribute(typeof(UpdateAttribute));

            return attribute.UpdateType;
        }
    }
}