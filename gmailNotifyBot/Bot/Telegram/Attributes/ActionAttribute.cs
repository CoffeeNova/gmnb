using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class ActionAttribute : Attribute
    {
        public ActionAttribute(string action)
        {
            Action = action;
        }
        public string Action { get; set; }

        public static string GetActionValue(Action action)
        {
            var type = typeof(Action);
            var fieldInfoArr = type.GetFields();
            var fieldInfo = fieldInfoArr.First(i => i.Name == action.ToString());
            if (fieldInfo == null)
                throw new ArgumentException($"The class {type} has no field {action}.", nameof(action));
            var attribute = (ActionAttribute)fieldInfo.GetCustomAttribute(typeof(ActionAttribute));

            return attribute.Action;
        }
    }
}