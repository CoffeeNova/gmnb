using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards
{
    internal class TypedInlineKeyboardButton : InlineKeyboardButton
    {
        public TypedInlineKeyboardButton(InlineKeyboardType type, string value)
        {
            Type = type;
            switch (type)
            {
                case InlineKeyboardType.Url:
                    base.Url = value;
                    break;
                case InlineKeyboardType.CallbackData:
                    base.CallbackData = value;
                    break;
                case InlineKeyboardType.SwitchInlineQuery:
                    base.SwitchInlineQuery = value;
                    break;
                case InlineKeyboardType.SwitchInlineQueryCurrentChat:
                    base.SwitchInlineQueryCurrentChat = value;
                    break;
            }
        }

        public InlineKeyboardType Type { get; }
    }

    internal enum InlineKeyboardType
    {
        Url,
        CallbackData,
        SwitchInlineQuery,
        SwitchInlineQueryCurrentChat
    } 
}