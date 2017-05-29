using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal abstract class GetKeyboard : Keyboard
    {
        protected GetKeyboard(FormattedMessage message)
        {
            message.NullInspect(nameof(message));
            Message = message;
        }

        public override void CreateInlineKeyboard()
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

        protected readonly FormattedMessage Message;
        protected GetCallbackData GeneralCallbackData;
        protected abstract GetKeyboardState State { get; }
        public int Page { get; set; }
        public bool IsIgnored { get; set; }
    }
}