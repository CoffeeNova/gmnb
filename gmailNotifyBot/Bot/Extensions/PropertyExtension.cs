
namespace CoffeeJelly.gmailNotifyBot.Bot.Extensions
{
    public static class PropertyExtension
    {
        public static void SetPropertyValue(this object obj, string propName, object value)
        {
            obj.GetType().GetProperty(propName).SetValue(obj, value, null);
        }

        public static object GetPropertyValue(this object obj, string propName)
        {
            return obj.GetType().GetProperty(propName).GetValue(obj, null);
        }
    }

    public static class IsNullExtension
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
    }
}