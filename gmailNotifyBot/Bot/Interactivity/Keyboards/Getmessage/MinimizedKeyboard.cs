using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal class MinimizedKeyboard : Keyboard
    {
        public MinimizedKeyboard(FormattedMessage message) : base(message)
        {
            InitButtons();
        }

        private void InitButtons()
        {
            ExpandButton = InitButton(ExpandButtonCaption, ExpandButtonCommand);
            ActionsButton = InitButton(ActionButtonCaption, ActionsButtonCommand);
            AttachmentsButton = InitButton(AttachmentsButtonCaption, AttachmentsButtonCommand);
        }

        protected override MessageKeyboardState State => MessageKeyboardState.Minimized;

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            FirstRow = new List<InlineKeyboardButton> { ExpandButton, ActionsButton, AttachmentsButton };
            var inlineKeyboard = new List<List<InlineKeyboardButton>> { FirstRow };
            return inlineKeyboard;
        }

        protected InlineKeyboardButton ExpandButton { get; set; }
        protected InlineKeyboardButton ActionsButton { get; set; }
        protected InlineKeyboardButton AttachmentsButton { get; set; }

        protected List<InlineKeyboardButton> FirstRow;

        private static string ExpandButtonCaption => MainButtonCaption.Expand;
        private static string ActionButtonCaption => MainButtonCaption.Actions;
        private static string AttachmentsButtonCaption => MainButtonCaption.Attachments;

        private static string ExpandButtonCommand => Commands.EXPAND_COMMAND;
        private static string ActionsButtonCommand => Commands.EXPAND_ACTIONS_COMMAND;
        private static string AttachmentsButtonCommand => Commands.SHOW_ATTACHMENTS_COMMAND;

    }


}