using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage
{
    internal static class SendKeyboardButtonCapton
    {
        public static string To => $"{Emoji.BOY_AND_GIRL}Add Recipients";

        public static string Cc => $"{Emoji.BOY_AND_GIRL}CC Recipients";

        public static string Bcc => $"{Emoji.MALE_FEMALE_SHADOWS}BCC Recipients";

        public static string Subject => $"{Emoji.ABC_LOWER_CASE}Subject";

        public static string Message => $"{Emoji.M}Message";

        public static string ChangeSubject => $"{Emoji.ABC_LOWER_CASE}Change Subject";

        public static string ChangeMessage => $"{Emoji.M}Change Message";

        public static string Save => $"{Emoji.FLOPPY_DISK}Save";

        public static string NotSave => $"{Emoji.WASTE_BASKET}Not save";

        public static string ContinueOld => $"{Emoji.MEMO}Continue with old";

        public static string Send => $"{Emoji.INCOMING_ENVELOPE} Send";

        public static string ToDraft => $"{Emoji.OUTBOX_TRAY} Save as Draft";

        public static string Continue => $"{Emoji.INBOX_TRAY} Continue";
    }
}