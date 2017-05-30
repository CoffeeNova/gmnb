using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal static class ActionButtonCaption
    {
        public static string ToRead => $"{Emoji.Eye} To Read";
        public static string ToUnread => $"{Emoji.RedArrowedEnvelope} To Unread";
        public static string NotSpam => $"{Emoji.HeartEnvelope} Not Spam";
        public static string Spam => $"{Emoji.RestrictionSign} Spam";
        public static string Restore => $"{Emoji.ClosedMailbox} Restore";
        public static string Delete => $"{Emoji.WasteBasket} Delete";
        public static string ToArchive => $"{Emoji.Multifolder} To Archive";
        public static string ToInbox => $"{Emoji.ClosedMailbox} To Inbox";
        public static string Unignore => "Unignore";
        public static string Ignore => "Ignore";
    }

    internal static class MainButtonCaption
    {
        public static string Expand => $"{Emoji.DownTriangle}Expand";
        public static string Actions => $"{Emoji.TurnedDownArrow} Actions";
        public static string Attachments => $"{Emoji.OpenFileFolder}Attachments";
        public static string Hide => $"{Emoji.UpTriangle} Hide";
        public static string PressedActions => $"{Emoji.TurnedUpArrow} Actions";
        public static string Close => "Close";

    }
}