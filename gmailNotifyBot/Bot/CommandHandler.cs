using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public sealed class CommandHandler
    {
        private CommandHandler(string token, UpdatesHandler updatesHandler, Secrets clientSecret)
        {
        }

        public static CommandHandler GetInstance(string token, UpdatesHandler updatesHandler, Secrets clientSecret)
        {
            if (Instance == null)
            {
                lock (_locker)
                {
                    if (Instance == null)
                        Instance = new CommandHandler(token, updatesHandler, clientSecret);
                }
            }
            return Instance;
        }

        private UpdatesHandler _updatesHandler;
        private readonly TelegramMethods _telegramMethods;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private static readonly object _locker = new object();

        public static CommandHandler Instance { get; private set; }

        public string ConnectStringCommand { get; set; } = @"/connect";
        public string SettingsStringCommand { get; set; } = @"/settings";

        public Secrets ClientSecret { get; set; }


    }
}