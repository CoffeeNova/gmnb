using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal static class ActionButtonCaption
    {
        public static string ToRead => $"{Emoji.EYE} To Read";
        public static string ToUnread => $"{Emoji.RED_ARROWED_ENVELOPE} To Unread";
        public static string NotSpam => $"{Emoji.HEART_ENVELOPE} Not Spam";
        public static string Spam => $"{Emoji.RESTRICTION_SIGN} Spam";
        public static string Restore => $"{Emoji.CLOSED_MAILBOX} Restore";
        public static string Delete => $"{Emoji.WASTE_BASKET} Delete";
        public static string ToArchive => $"{Emoji.MULTIFOLDER} To Archive";
        public static string ToInbox => $"{Emoji.CLOSED_MAILBOX} To Inbox";
        public static string Unignore => "Unignore";
        public static string Ignore => "Ignore";
        public static string OpenWeb => $"{Emoji.GLOBE} Open in web browser";
    }

    internal static class MainButtonCaption
    {
        public static string Expand => $"{Emoji.DOWN_TRIANGLE}Expand";
        public static string Actions => $"{Emoji.TURNED_DOWN_ARROW} Actions";
        public static string Attachments => $"{Emoji.OPEN_FILE_FOLDER}Attachments";
        public static string Hide => $"{Emoji.UP_TRIANGLE} Hide";
        public static string PressedActions => $"{Emoji.TURNED_UP_ARROW} Actions";
        public static string Close => "Close";

    }
}