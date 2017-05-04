using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public sealed class BotSettings
    {
        public static BotSettings GetInstance()
        {
            if (Instance == null)
            {
                lock (_locker)
                {
                    if (Instance == null)
                        Instance = new BotSettings();
                }
            }
            return Instance;

        }

        private BotSettings()
        {
            
        }
        public bool AllSettingsAreSet()
        {
            var propertiesInfo = this.GetType().GetProperties(BindingFlags.Instance |  BindingFlags.Public);
            return propertiesInfo.All(p => p.GetValue(this) != null);
        }

        private static readonly object _locker = new object();

        public static BotSettings Instance { get; private set;}

        public string Username { get; set; }
    }
}