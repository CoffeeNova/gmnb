using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal class MinimizedKeyboard : Keyboard
    {
        internal MinimizedKeyboard(FormattedMessage message) : base(message)
        {

        }

        protected override void ButtonsInitializer()
        {
            ExpandButton = InitButton(ExpandButtonCaption, ExpandButtonCommand);
            ActionsButton = InitButton(ActionButtonCaption, ActionsButtonCommand);
            AttachmentsButton = InitButton(AttachmentsButtonCaption, AttachmentsButtonCommand);
        }

        protected override GetKeyboardState State => GetKeyboardState.Minimized;

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            MainRow = new List<InlineKeyboardButton>();
            if (ExpandButton != null)
                MainRow.Add(ExpandButton);
            if (ActionsButton != null)
                MainRow.Add(ActionsButton);
            if (Message.HasAttachments && AttachmentsButton != null)
                MainRow.Add(AttachmentsButton);
            var inlineKeyboard = new List<List<InlineKeyboardButton>> { MainRow };
            return inlineKeyboard;
        }

        protected InlineKeyboardButton ExpandButton { get; set; }
        protected InlineKeyboardButton ActionsButton { get; set; }
        protected InlineKeyboardButton AttachmentsButton { get; set; }

        protected List<InlineKeyboardButton> MainRow;

        private static string ExpandButtonCaption => MainButtonCaption.Expand;
        private static string ActionButtonCaption => MainButtonCaption.Actions;
        private static string AttachmentsButtonCaption => MainButtonCaption.Attachments;

        private static string ExpandButtonCommand => Commands.EXPAND_COMMAND;
        private static string ActionsButtonCommand => Commands.EXPAND_ACTIONS_COMMAND;
        private static string AttachmentsButtonCommand => Commands.SHOW_ATTACHMENTS_COMMAND;

    }


}