using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class UserSettingsModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public bool MailNotification { get; set; }
    }
}