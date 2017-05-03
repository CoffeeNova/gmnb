using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram.Extensions
{
    public static class IntExtension
    {
        public static bool InRange(this int numb, int min, int max)
        {
            return numb >= min && numb <= max;
        }
    }
}