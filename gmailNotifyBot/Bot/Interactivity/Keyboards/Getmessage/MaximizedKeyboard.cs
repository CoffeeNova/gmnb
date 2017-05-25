using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal class MaximizedKeyboard : Keyboard
    {
        internal MaximizedKeyboard(FormattedMessage message) : base(message)
        {
        }

        protected override void ButtonsInitializer()
        {
            ExpandButton = InitButton(ExpandButtonCaption, ExpandButtonCommand);
            ActionsButton = InitButton(ActionButtonCaption, ActionsButtonCommand);
            AttachmentsButton = InitButton(AttachmentsButtonCaption, AttachmentsButtonCommand);
        }

        protected override MessageKeyboardState State => MessageKeyboardState.Maximized;

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            SliderRow = PageSliderRow();
            MainRow = new List<InlineKeyboardButton>();
            if (ExpandButton != null)
                MainRow.Add(ExpandButton);
            if (ActionsButton != null)
                MainRow.Add(ActionsButton);
            if (Message.HasAttachments)
                MainRow.Add(AttachmentsButton);

            var inlineKeyboard = new List<List<InlineKeyboardButton>> { SliderRow, MainRow };
            return inlineKeyboard;
        }

        protected InlineKeyboardButton ExpandButton { get; set; }
        protected InlineKeyboardButton ActionsButton { get; set; }
        protected InlineKeyboardButton AttachmentsButton { get; set; }

        protected List<InlineKeyboardButton> SliderRow;
        protected List<InlineKeyboardButton> MainRow;

        private string ExpandButtonCaption => MainButtonCaption.Hide;
        private string ActionButtonCaption => MainButtonCaption.Actions;
        private string AttachmentsButtonCaption => MainButtonCaption.Attachments;

        private string ExpandButtonCommand => Commands.HIDE_COMMAND;
        private string ActionsButtonCommand => Commands.EXPAND_ACTIONS_COMMAND;
        private string AttachmentsButtonCommand => Commands.SHOW_ATTACHMENTS_COMMAND;

    }


}