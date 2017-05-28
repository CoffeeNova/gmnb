﻿using System;
using System.Linq;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    internal static class Tools
    {
        internal static string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}