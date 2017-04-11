using System;
using System.Diagnostics;
using System.Reflection;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TelegramMethodAttribute : Attribute
    {
        public TelegramMethodAttribute(string methodName, string fileType)
        {
            MethodName = methodName;
            FileType = fileType;
        }

        public TelegramMethodAttribute(string methodName)
        {
            MethodName = methodName;
        }

        public string MethodName { get; set; }

        public string FileType { get; set; }

        public static string GetMethodNameValue(Type classType, string callerMethodName)
        {
            MethodInfo method = classType.GetMethod(callerMethodName);
            var attributes = (TelegramMethodAttribute[])method.GetCustomAttributes(typeof(TelegramMethodAttribute), true);

            string methodName = "";
            if (attributes.Length > 0)
                methodName = attributes[0].MethodName;
            return methodName;
        }

        public static string GetFileTypeValue(Type classType, string callerMethodName)
        {
            var method = classType.GetMethod(callerMethodName);
            var attributes = (TelegramMethodAttribute[])method.GetCustomAttributes(typeof(TelegramMethodAttribute), true);

            string fileType = "";
            if (attributes.Length > 0)
                fileType = attributes[0].FileType;
            return fileType;
        }
    }

}