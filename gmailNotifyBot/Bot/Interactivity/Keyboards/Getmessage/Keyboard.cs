using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal abstract class Keyboard : InlineKeyboardMarkup
    {
        protected Keyboard(FormattedMessage message)
        {
            message.NullInspect(nameof(message));
            Message = message;
            
        }

        public void CreateInlineKeyboard()
        {
            GeneralCallbackData = new GetCallbackData
            {
                MessageId = Message.Id,
                Page = Page,
                MessageKeyboardState = State
            };
            ButtonsInitializer();
            base.InlineKeyboard = DefineInlineKeyboard();
        }

        /// <summary>
        /// This method fires before when called <see cref="CreateInlineKeyboard"/> and should be used to initialize keyboard buttons.
        /// </summary>
        protected abstract void ButtonsInitializer();
        protected abstract IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="callbackCommand"></param>
        /// <returns>Do not use virtual members from constructor!</returns>
        protected virtual InlineKeyboardButton InitButton(string text, string callbackCommand)
        {
            return new InlineKeyboardButton
            {
                Text = text,
                CallbackData = new GetCallbackData(GeneralCallbackData)
                {
                    Command = callbackCommand
                }
            };
        }

        protected List<InlineKeyboardButton> PageSliderRow()
        {
            var row = new List<InlineKeyboardButton>();
            InlineKeyboardButton nextPageButton = null;
            InlineKeyboardButton prevPageButton = null;
            if (Message.MultiPageBody)
            {
                var pageCount = Message.Pages;
                if (Page < pageCount)
                {
                    nextPageButton = new InlineKeyboardButton();
                    nextPageButton.Text = $"To Page {Page + 1} {Emoji.RightArrow}";
                    nextPageButton.CallbackData = new GetCallbackData(GeneralCallbackData)
                    {
                        Command = Commands.NEXTPAGE_COMMAND
                    };
                }
            }
            if (Page > 1)
            {
                prevPageButton = new InlineKeyboardButton();
                prevPageButton.Text = $"{Emoji.LeftArrow} To Page {Page - 1}";
                prevPageButton.CallbackData = new GetCallbackData(GeneralCallbackData)
                {
                    Command = Commands.PREVPAGE_COMMAND
                };
            }
            if (prevPageButton != null)
                row.Add(prevPageButton);
            if (nextPageButton != null)
                row.Add(nextPageButton);
            return row;
        }

        protected readonly FormattedMessage Message;
        protected GetCallbackData GeneralCallbackData;
        protected abstract GetKeyboardState State { get; }
        public int Page { get; set; }
        public bool IsIgnored { get; set; }
    }
}