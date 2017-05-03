using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Runtime.CompilerServices;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.Extensions;

[assembly: InternalsVisibleTo("ToolsTests")]
namespace CoffeeJelly.gmailNotifyBot.Bot.Telegram
{
    internal static class Tools
    {
        ///A-Z, a-z, 0-9, _ and - are allowed.
        public static bool OnlyAllowedCharacters1(string str)
        {
            var regex = new Regex(@"^[a-zA-Z0-9_-]*$");
            var match = regex.Match(str);
            return match.Success;
        }

    }

    
}