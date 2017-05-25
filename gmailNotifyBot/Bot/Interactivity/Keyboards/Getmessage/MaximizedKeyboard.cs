using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal class MaximizedKeyboard : Keyboard
    {
        public MaximizedKeyboard(FormattedMessage message, int page) : base(message, page)
        {
            InitButtons();
        }

        private void InitButtons()
        {
            ExpandButton = InitButton(ExpandButtonCaption, ExpandButtonCommand);
            ActionsButton = InitButton(ActionButtonCaption, ActionsButtonCommand);
            AttachmentsButton = InitButton(AttachmentsButtonCaption, AttachmentsButtonCommand);
        }

        protected override MessageKeyboardState State => MessageKeyboardState.Maximized;

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            FirstRow = PageSliderRow();
            SecondRow = new List<InlineKeyboardButton> { ExpandButton, ActionsButton, AttachmentsButton };
            var inlineKeyboard = new List<List<InlineKeyboardButton>> { FirstRow };
            return inlineKeyboard;
        }

        protected InlineKeyboardButton ExpandButton { get; set; }
        protected InlineKeyboardButton ActionsButton { get; set; }
        protected InlineKeyboardButton AttachmentsButton { get; set; }

        protected List<InlineKeyboardButton> FirstRow;
        protected List<InlineKeyboardButton> SecondRow;

        private string ExpandButtonCaption => MainButtonCaption.Hide;
        private string ActionButtonCaption => MainButtonCaption.Actions;
        private string AttachmentsButtonCaption => MainButtonCaption.Attachments;

        private string ExpandButtonCommand => Commands.HIDE_COMMAND;
        private string ActionsButtonCommand => Commands.EXPAND_ACTIONS_COMMAND;
        private string AttachmentsButtonCommand => Commands.SHOW_ATTACHMENTS_COMMAND;

    }


}