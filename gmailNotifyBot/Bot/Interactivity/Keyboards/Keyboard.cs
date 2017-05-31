using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards
{
    internal abstract class Keyboard : InlineKeyboardMarkup
    {
        public abstract void CreateInlineKeyboard();
        
        /// <summary>
        /// This method fires before when called <see cref="CreateInlineKeyboard"/> and should be used to initialize keyboard buttons.
        /// </summary>
        protected abstract void ButtonsInitializer();

        protected abstract IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        /// <param name="actionValue"></param>
        /// <remarks>Do not use virtual members from constructor!</remarks>
        protected virtual InlineKeyboardButton InitButton(InlineKeyboardType type, string text, string actionValue)
        {
            return new TypedInlineKeyboardButton(type, actionValue)
            {
                Text = text
            };
        }
    }
}