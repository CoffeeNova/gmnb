using System.Text;

namespace CoffeeJelly.TelegramBotApiWrapper.Extensions
{
    public static class StringExtension
    {
        public static int SizeUtf8(this string str)
        {
            return Encoding.UTF8.GetByteCount(str);
        }
    }
}