using System;

namespace CoffeeJelly.TelegramApiWrapper.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    internal class TelegramMethodAttribute : Attribute
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
            if (string.IsNullOrEmpty(callerMethodName))
                throw new ArgumentNullException(nameof(callerMethodName), "Should have a value.");

            var method = classType.GetMethod(callerMethodName);
            if (method == null)
                throw new ArgumentException($"The class {classType} has no method {callerMethodName}.", nameof(callerMethodName));
        
            var attributes = (TelegramMethodAttribute[])method.GetCustomAttributes(typeof(TelegramMethodAttribute), true);

            string methodName = "";
            if (attributes.Length > 0)
                methodName = attributes[0].MethodName;
            return methodName;
        }

        public static string GetFileTypeValue(Type classType, string callerMethodName)
        {
            if (string.IsNullOrEmpty(callerMethodName))
                throw new ArgumentNullException(nameof(callerMethodName), "Should have a value.");

            var method = classType.GetMethod(callerMethodName);
            if (method == null)
                throw new ArgumentException($"The class {classType} has no method {callerMethodName}.", nameof(callerMethodName));
            var attributes = (TelegramMethodAttribute[])method.GetCustomAttributes(typeof(TelegramMethodAttribute), true);

            string fileType = "";
            if (attributes.Length > 0)
                fileType = attributes[0].FileType;
            return fileType;
        }
    }

}