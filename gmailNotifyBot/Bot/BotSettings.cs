using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using CoffeeJelly.gmailNotifyBot.Bot.Types;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class BotSettings
    {
        public bool AllSettingsAreSet()
        {
            var propertiesInfo = this.GetType().GetProperties(BindingFlags.Instance |  BindingFlags.Public);
            return propertiesInfo.All(p => p.GetValue(this) != null);
        }

        public string Token { get; set; }

        public Secrets ClientSecrets { get; set; }

        public string Username { get; set; }

        public string Topic { get; set; }

        public string Subscription { get; set; }
    }
}