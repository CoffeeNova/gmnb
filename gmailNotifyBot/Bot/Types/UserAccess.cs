using CoffeeJelly.gmailNotifyBot.Bot.Attributes;

namespace CoffeeJelly.gmailNotifyBot.Bot.Types
{
    public static class UserAccess
    {
        [UserAccess(@"https://mail.google.com/", @"https://www.googleapis.com/auth/gmail.compose", @"https://www.googleapis.com/auth/userinfo.email")]
        public const string FULL = "full";

        [UserAccess(@"https://www.googleapis.com/auth/gmail.metadata", @"https://www.googleapis.com/auth/userinfo.email")]
        public const string NOTIFY = "notify";
    }
}