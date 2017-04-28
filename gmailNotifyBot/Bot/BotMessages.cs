using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public class BotMessages
    {
        public BotMessages(string token)
        {
            _telegramMethods = new TelegramMethods(token);
        }

        public async void WrongCredentialsMessage(string chatId)
        {
            await _telegramMethods.SendMessageAsync(chatId, @"I am lost your credentials. Please reauthorize me using /connect command or click a button below.", null, false, false, null, ReauthorizeButton);
        }

        private static InlineKeyboardMarkup ReauthorizeButton
        {
            get
            {
                var reauthorizeButton = new InlineKeyboardButton
                {
                    Text = "Reauthorize",
                    CallbackData = Commands.CONNECT_STRING_COMMAND
                };

                return new InlineKeyboardMarkup
                {
                    InlineKeyboard = new List<List<InlineKeyboardButton>>
                    {
                        new List<InlineKeyboardButton>
                        {
                            reauthorizeButton
                        }
                    }
                };
            }
        }

        public async void AuthorizeMessage(string chatId, Uri notifyAccessUri, Uri fullAccessUri)
        {
            var notifyAccessButton = new InlineKeyboardButton
            {
                Text = "Mail Notify",
                Url = notifyAccessUri.OriginalString
            };
            var fullAccessButton = new InlineKeyboardButton
            {
                Text = "Full Access",
                Url = fullAccessUri.OriginalString
            };
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton>
                    {
                        notifyAccessButton,
                        fullAccessButton
                    }
                }
            };
            await _telegramMethods.SendMessageAsync(chatId,
                $"Open one of this link to authorize the bot to get: ", null, false, false, null, keyboard);
        }
        private static TelegramMethods _telegramMethods;

    }
}