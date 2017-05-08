using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Extensions
{
    public static class EnumExtension
    {
        public static string ToEnumString<T>(this T subj)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Must be enum type", nameof(T));

            var enumType = typeof(T);
            var name = Enum.GetName(enumType, subj);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            return enumMemberAttribute.Value;
        }

        public static T ToEnum<T>(this string str)
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Must be enum type", nameof(T));

            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
            }
            throw new ArgumentException($"Must be a value of EnumMemberAttribute of {enumType}", nameof(str));
        }
    }
}