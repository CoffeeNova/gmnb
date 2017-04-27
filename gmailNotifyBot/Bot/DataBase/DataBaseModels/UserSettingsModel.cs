using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Attributes;


namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class UserSettingsModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public bool MailNotification { get; set; } = true;

        public string Access { get; set; } = UserAccess.Full;
    }

    public static class UserAccess
    {
        [UserAccess(@"https://mail.google.com/", @"https://www.googleapis.com/auth/gmail.compose", @"https://www.googleapis.com/auth/userinfo.profile")]
        public static readonly string Full = "full";

        [UserAccess(@"https://www.googleapis.com/auth/gmail.labels", @"https://www.googleapis.com/auth/userinfo.profile")]
        public static readonly string Notify = "notify";
    }
}