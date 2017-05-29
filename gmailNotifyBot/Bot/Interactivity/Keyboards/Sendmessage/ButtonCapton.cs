using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage
{
    internal static class SendButtonCapton
    {
        public static string To => $"{Emoji.BoyAndGirl}Add Recipients";

        public static string Cc => $"{Emoji.BoyAndGirl}CC Recipients";

        public static string Bcc => $"{Emoji.MaleFemaleShadows}BCC Recipients";

        public static string Subject => $"{Emoji.AbcLowerCase}Subject";

        public static string Message => $"{Emoji.M}Message";
    }
}