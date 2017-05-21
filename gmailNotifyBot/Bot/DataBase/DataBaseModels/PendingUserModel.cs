using System;
using System.Data.Entity;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class PendingUserModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime JoinTimeUtc { get; set; }

    }
}