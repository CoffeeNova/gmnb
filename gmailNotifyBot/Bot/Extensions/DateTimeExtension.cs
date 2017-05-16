using System;

namespace CoffeeJelly.gmailNotifyBot.Bot.Extensions
{
    public static class DateTimeExtension
    {
        public static TimeSpan Difference(this DateTime date, DateTime date2)
        {
            return date2 - date;
        }

        public static TimeSpan Difference(this DateTime date, long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(unixTime);
            return  epoch - date;
        }

    }
}