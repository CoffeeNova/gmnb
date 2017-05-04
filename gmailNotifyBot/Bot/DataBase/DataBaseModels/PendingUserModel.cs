using System;
using System.Data.Entity;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class PendingUserModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public DateTime JoinTimeUtc { get; set; }

    }
}