using System.Collections.Generic;
using System.Linq;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal class MaximizedActionsKeyboard : MaximizedKeyboard
    {
        internal MaximizedActionsKeyboard(FormattedMessage message) : base(message)
        {
        }

        protected override void ButtonsInitializer()
        {
            base.ButtonsInitializer();
            ExpandButton = InitButton(ExpandButtonCaption, ExpandButtonCommand);
            ActionsButton = InitButton(ActionsButtonCaption, ActionsButtonCommand);
            UnreadButton = Message.LabelIds.Exists(l => l == Label.Unread) 
                ? InitButton(ActionButtonCaption.ToRead, CallbackCommand.TO_READ_COMMAND) 
                : InitButton(ActionButtonCaption.ToUnread, CallbackCommand.TO_UNREAD_COMMAND);
            SpamButton = Message.LabelIds.Exists(l => l == Label.Spam)
                ? InitButton(ActionButtonCaption.NotSpam, CallbackCommand.TO_INBOX_COMMAND)
                : InitButton(ActionButtonCaption.Spam, CallbackCommand.TO_SPAM_COMMAND);
            TrashButton = Message.LabelIds.Exists(l => l == Label.Trash)
                ? InitButton(ActionButtonCaption.Restore, CallbackCommand.TO_INBOX_COMMAND)
                : InitButton(ActionButtonCaption.Delete, CallbackCommand.TO_TRASH_COMMAND);
            ArchiveButton = Message.LabelIds.Exists(l => l == Label.Inbox)
                ? InitButton(ActionButtonCaption.ToArchive, CallbackCommand.ARCHIVE_COMMAND)
                : InitButton(ActionButtonCaption.ToInbox, CallbackCommand.TO_INBOX_COMMAND);
            NotifyButton = IsIgnored
                ? InitButton(ActionButtonCaption.Unignore, CallbackCommand.UNIGNORE_COMMAND)
                : InitButton(ActionButtonCaption.Ignore, CallbackCommand.IGNORE_COMMAND);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            ActionsRow = new List<InlineKeyboardButton>();
            FillActionsRow();
            var inlineKeyboard = base.DefineInlineKeyboard().ToList();
            inlineKeyboard.Add(ActionsRow);
            return inlineKeyboard;
        }

        private void FillActionsRow()
        {
            if (UnreadButton != null)
                ActionsRow.Add(UnreadButton);
            if (SpamButton != null)
                ActionsRow.Add(SpamButton);
            if (TrashButton != null)
                ActionsRow.Add(TrashButton);
            if (ArchiveButton != null)
                ActionsRow.Add(ArchiveButton);
            if (NotifyButton != null)
                ActionsRow.Add(NotifyButton);
        }

        protected override GetKeyboardState State => GetKeyboardState.MaximizedActions;

        protected virtual InlineKeyboardButton UnreadButton { get; set; }
        protected virtual InlineKeyboardButton SpamButton { get; set; }
        protected virtual InlineKeyboardButton TrashButton { get; set; }
        protected virtual InlineKeyboardButton ArchiveButton { get; set; }
        protected virtual InlineKeyboardButton NotifyButton { get; set; }

        protected List<InlineKeyboardButton> ActionsRow = new List<InlineKeyboardButton>();

        private static string ExpandButtonCaption => MainButtonCaption.Hide;

        private static string ExpandButtonCommand => CallbackCommand.HIDE_COMMAND;

        private static string ActionsButtonCaption => MainButtonCaption.PressedActions;

        private static string ActionsButtonCommand => CallbackCommand.HIDE_ACTIONS_COMMAND;

    }

}