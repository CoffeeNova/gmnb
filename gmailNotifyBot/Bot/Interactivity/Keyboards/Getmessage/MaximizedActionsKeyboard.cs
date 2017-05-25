using System.Collections.Generic;
using System.Linq;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal class MazimizedActionsKeyboard : MaximizedKeyboard
    {
        public MazimizedActionsKeyboard(FormattedMessage message, int page, bool isIgnored) : base(message, page)
        {
            IsIgnored = isIgnored;
            InitButtons();
        }

        private void InitButtons()
        {
            ExpandButton = InitButton(ExpandButtonCaption, ExpandButtonCommand);
            ActionsButton = InitButton(ActionsButtonCaption, ActionsButtonCommand);
            UnreadButton = Message.LabelIds.Exists(l => l == Label.Unread) 
                ? InitButton(ActionButtonCaption.ToRead, Commands.TO_READ_COMMAND) 
                : InitButton(ActionButtonCaption.ToUnread, Commands.TO_UNREAD_COMMAND);
            SpamButton = Message.LabelIds.Exists(l => l == Label.Spam)
                ? InitButton(ActionButtonCaption.NotSpam, Commands.TO_INBOX_COMMAND)
                : InitButton(ActionButtonCaption.Spam, Commands.TO_SPAM_COMMAND);
            TrashButton = Message.LabelIds.Exists(l => l == Label.Trash)
                ? InitButton(ActionButtonCaption.Restore, Commands.TO_INBOX_COMMAND)
                : InitButton(ActionButtonCaption.Delete, Commands.TO_TRASH_COMMAND);
            ArchiveButton = Message.LabelIds.Exists(l => l == Label.Inbox)
                ? InitButton(ActionButtonCaption.ToArchive, Commands.ARCHIVE_COMMAND)
                : InitButton(ActionButtonCaption.ToInbox, Commands.TO_INBOX_COMMAND);
            NotifyButton = IsIgnored
                ? InitButton(ActionButtonCaption.Unignore, Commands.UNIGNORE_COMMAND)
                : InitButton(ActionButtonCaption.Ignore, Commands.IGNORE_COMMAND);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            ThirdRow = new List<InlineKeyboardButton> { UnreadButton, SpamButton, TrashButton, ArchiveButton, NotifyButton };
            var inlineKeyboard = base.DefineInlineKeyboard().ToList();
            inlineKeyboard.Add(ThirdRow);
            return inlineKeyboard;
        }

        protected override MessageKeyboardState State => MessageKeyboardState.MinimizedActions;

        protected virtual InlineKeyboardButton UnreadButton { get; set; }
        protected virtual InlineKeyboardButton SpamButton { get; set; }
        protected virtual InlineKeyboardButton TrashButton { get; set; }
        protected virtual InlineKeyboardButton ArchiveButton { get; set; }
        protected virtual InlineKeyboardButton NotifyButton { get; set; }

        protected List<InlineKeyboardButton> ThirdRow = new List<InlineKeyboardButton>();
        protected bool IsIgnored { get; set; }

        private static string ExpandButtonCaption => MainButtonCaption.Hide;

        private static string ExpandButtonCommand => Commands.HIDE_COMMAND;

        private static string ActionsButtonCaption => MainButtonCaption.PressedActions;

        private static string ActionsButtonCommand => Commands.HIDE_ACTIONS_COMMAND;

    }

}