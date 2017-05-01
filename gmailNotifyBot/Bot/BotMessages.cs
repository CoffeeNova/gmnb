using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task WrongCredentialsMessage(string userId)
        {
            await _telegramMethods.SendMessageAsync(userId, @"I am lost your credentials. Please reauthorize me using /connect command or click a button below.", null, false, false, null, ReauthorizeButton);
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

        public async Task AuthorizeMessage(string userId, Uri notifyAccessUri, Uri fullAccessUri)
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
            await _telegramMethods.SendMessageAsync(userId,
                $"Open one of this link to authorize the bot to get: ", null, false, false, null, keyboard);
        }

        public async Task AuthorizationTimeExpiredMessage(string userId)
        {
            await
                _telegramMethods.SendMessageAsync(userId,
                    @"Time for authorization has expired. Please type again /connect command.");
        }

        public async Task AuthorizationFailedMessage(string userId)
        {
            await
                _telegramMethods.SendMessageAsync(userId, "Authorization failed. See ya!");
        }

        public async Task AuthorizationSuccessfulMessage(string userId)
        {
            await
                _telegramMethods.SendMessageAsync(userId, "Authorization successful! Now you can recieve notifications about new emails and use other functions!");
        }

        public async Task EmailAddressMessage(string userId, string emailAddress)
        {
            await _telegramMethods.SendMessageAsync(userId, emailAddress);
        }
        private static TelegramMethods _telegramMethods;

    }
}