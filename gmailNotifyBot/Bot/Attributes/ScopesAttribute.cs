using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;

namespace CoffeeJelly.gmailNotifyBot.Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class UserAccessAttribute : Attribute
    {
        public UserAccessAttribute(params string[] scopes)
        {
            Scopes = scopes?.ToList();
        }
        public List<string> Scopes { get; set; }

        public static List<string> GetScopesValue(string userAccess)
        {
            if(string.IsNullOrEmpty(userAccess))
                throw new ArgumentNullException(nameof(userAccess), "Should have a value.");

            var type = typeof(UserAccess);
            var fieldInfoArr = type.GetFields();
            var fieldInfo = fieldInfoArr.FirstOrDefault(i => i.GetValue(null).ToString() == userAccess);
            if (fieldInfo == null)
                throw new ArgumentException($"The class {type} has no field {userAccess}.", nameof(userAccess));
            var attribute = (UserAccessAttribute)fieldInfo.GetCustomAttribute(typeof(UserAccessAttribute));

            return attribute.Scopes;
        }
    }
}