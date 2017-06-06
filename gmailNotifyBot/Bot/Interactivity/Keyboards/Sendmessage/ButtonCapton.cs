using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage
{
    internal static class SendKeyboardButtonCapton
    {
        public static string To => $"{Emoji.BoyAndGirl}Add Recipients";

        public static string Cc => $"{Emoji.BoyAndGirl}CC Recipients";

        public static string Bcc => $"{Emoji.MaleFemaleShadows}BCC Recipients";

        public static string Subject => $"{Emoji.AbcLowerCase}Subject";

        public static string Message => $"{Emoji.M}Message";

        public static string ChangeSubject => $"{Emoji.AbcLowerCase}Change Subject";

        public static string ChangeMessage => $"{Emoji.M}Change Message";

        public static string Save => $"{Emoji.FloppyDisk}Save";

        public static string NotSave => $"{Emoji.WasteBasket}Not save";

        public static string ContinueOld => $"{Emoji.Memo}Continue with old";

        public static string Send => $"{Emoji.IncomingEnvelope} Send";

        public static string ToDraft => $"{Emoji.OutboxTray} Save as Draft";

        public static string Continue => $"{Emoji.InboxTray} Continue";
    }
}