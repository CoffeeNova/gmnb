using System;
using System.Linq;
using System.Reflection;
using CoffeeJelly.TelegramApiWrapper.Types;

namespace CoffeeJelly.TelegramApiWrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class ChatActionAttribute : Attribute
    {
        public ChatActionAttribute(string action)
        {
            Action = action;
        }
        public string Action { get; set; }

        public static string GetActionValue(ChatAction action)
        {
            var type = typeof(ChatAction);
            var fieldInfoArr = type.GetFields();
            var fieldInfo = fieldInfoArr.First(i => i.Name == action.ToString());
            if (fieldInfo == null)
                throw new ArgumentException($"The class {type} has no field {action}.", nameof(action));
            var attribute = (ChatActionAttribute)fieldInfo.GetCustomAttribute(typeof(ChatActionAttribute));

            return attribute.Action;
        }
    }
}