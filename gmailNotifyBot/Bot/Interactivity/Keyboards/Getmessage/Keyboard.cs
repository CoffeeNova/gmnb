using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal abstract class Keyboard : InlineKeyboardMarkup
    {
        internal Keyboard(FormattedMessage message, int page = 0)
        {
            message.NullInspect(nameof(message));

            Message = message;
            Page = page;
        }

        public void CreateInlineKeyboard()
        {
            GeneralCallbackData = new CallbackData
            {
                MessageId = Message.Id,
                Page = Page,
                MessageKeyboardState = State
            };
            base.InlineKeyboard = DefineInlineKeyboard();

        }

        protected abstract IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard();

        protected virtual InlineKeyboardButton InitButton(string text, string callbackCommand)
        {
            return new InlineKeyboardButton
            {
                Text = text,
                CallbackData = new CallbackData(GeneralCallbackData)
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
                    nextPageButton.CallbackData = new CallbackData(GeneralCallbackData)
                    {
                        Command = Commands.NEXTPAGE_COMMAND
                    };
                }
            }
            if (Page > 1)
            {
                prevPageButton = new InlineKeyboardButton();
                prevPageButton.Text = $"{Emoji.LeftArrow} To Page {Page - 1}";
                prevPageButton.CallbackData = new CallbackData(GeneralCallbackData)
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
        protected CallbackData GeneralCallbackData;
        protected abstract MessageKeyboardState State { get; }
        protected int Page;
    }
}