using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Extensions
{
    public static class StringExtension
    {
        public static int SizeUtf8(this string str)
        {
            return Encoding.UTF8.GetByteCount(str);
        }
    }
}