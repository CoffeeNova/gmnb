using System;
using System.Data.Entity;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class PendingUserModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public DateTime JoinTimeUtc { get; set; }

    }
}