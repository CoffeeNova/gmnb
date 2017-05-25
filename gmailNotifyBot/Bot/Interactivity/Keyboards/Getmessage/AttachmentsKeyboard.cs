using System.Collections.Generic;
using System.Linq;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal class AttachmentsKeyboard : Keyboard
    {
        public AttachmentsKeyboard(FormattedMessage message, int page) : base(message, page)
        {
            InitButtons();
        }

        private void InitButtons()
        {
            CloseButton = new InlineKeyboardButton
            {
                Text = MainButtonCaption.Close,
                CallbackData = new CallbackData(GeneralCallbackData)
                {
                    Command = Commands.HIDE_ATTACHMENTS_COMMAND
                }
            };
            FirstRow = new List<InlineKeyboardButton>();
            Message.Attachments.IndexEach((a, i) =>
            {
                FirstRow.Add(new InlineKeyboardButton
                {
                    Text = $"{i + 1}. {a.FileName}",
                    CallbackData = new CallbackData(GeneralCallbackData)
                    {
                        Command = Commands.GET_ATTACHMENT_COMMAND + $"{i}"
                    }
                });
            });
            FirstRow.Add(CloseButton);
        }

        protected override MessageKeyboardState State => MessageKeyboardState.Minimized;

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            return FirstRow.DivideByLength(4).ToList();
        }

        protected InlineKeyboardButton CloseButton { get; set; }

        protected List<InlineKeyboardButton> FirstRow;


    }


}