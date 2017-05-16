using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Attributes;


namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class UserSettingsModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool MailNotification { get; set; } = true;

        public ulong HistoryId { get; set; }

        public long Expiration { get; set; }

        public string Access { get; set; } = UserAccess.Full;

        public List<string> IgnoreList { get; set; } = new List<string>();


    }

    public static class UserAccess
    {
        [UserAccess(@"https://mail.google.com/", @"https://www.googleapis.com/auth/gmail.compose", @"https://www.googleapis.com/auth/userinfo.profile")]
        public static readonly string Full = "full";

        [UserAccess(@"https://www.googleapis.com/auth/gmail.labels", @"https://www.googleapis.com/auth/userinfo.profile")]
        public static readonly string Notify = "notify";
    }
}