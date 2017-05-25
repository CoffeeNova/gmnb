using System.Collections.Generic;
using System.Linq;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity
{
    internal class MessageInlineKeyboardMarkup : InlineKeyboardMarkup
    {
        internal MessageInlineKeyboardMarkup(FormattedMessage message, int page, MessageKeyboardState state,
            bool isIgnored)
        {
            message.NullInspect(nameof(message));

            _message = message;
            _isIgnored = isIgnored;
            _state = state;
            _page = page;
            _generalCallbackData = new CallbackData
            {
                MessageId = message.Id,
                Page = page,
                MessageKeyboardState = state,
            };

            SetMainButtonsState();
            base.InlineKeyboard = CreateInlineKeyboard();
        }

        private IEnumerable<IEnumerable<InlineKeyboardButton>> CreateInlineKeyboard()
        {
            if (_atachmentsKeyboard != null)
                return _atachmentsKeyboard;

            var inlineKeyboard = new List<List<InlineKeyboardButton>>();
            if (_firstRow.Count > 0)
                inlineKeyboard.Add(_firstRow);
            if (_secondRow.Count > 0)
                inlineKeyboard.Add(_secondRow);
            if (_thirdRow.Count > 0)
                inlineKeyboard.Add(_thirdRow);

            return inlineKeyboard;
        }

        private void SetMainButtonsState()
        {
            switch (_state)
            {
                case MessageKeyboardState.Minimized:
                    DefineMainButtonsProperties(MainButtonCaption.Expand, MainButtonCaption.Actions, MainButtonCaption.Attachments,
                    Commands.EXPAND_COMMAND, Commands.EXPAND_ACTIONS_COMMAND, Commands.SHOW_ATTACHMENTS_COMMAND);
                    DefineMinimizedRow();
                    break;
                case MessageKeyboardState.Maximized:
                    DefineMainButtonsProperties(MainButtonCaption.Hide, MainButtonCaption.Actions, MainButtonCaption.Attachments,
                    Commands.HIDE_COMMAND, Commands.EXPAND_ACTIONS_COMMAND, Commands.SHOW_ATTACHMENTS_COMMAND);
                    DefineMaximizedRow();
                    break;
                case MessageKeyboardState.MinimizedActions:
                    DefineMainButtonsProperties(MainButtonCaption.Expand, MainButtonCaption.PressedActions, MainButtonCaption.Attachments,
                    Commands.EXPAND_COMMAND, Commands.HIDE_ACTIONS_COMMAND, Commands.SHOW_ATTACHMENTS_COMMAND);
                    DefineMinimizedActionsRow();
                    break;
                case MessageKeyboardState.MaximizedActions:
                    DefineMainButtonsProperties(MainButtonCaption.Hide, MainButtonCaption.PressedActions, MainButtonCaption.Attachments,
                    Commands.HIDE_COMMAND, Commands.HIDE_ACTIONS_COMMAND, Commands.SHOW_ATTACHMENTS_COMMAND);
                    DefineMaximizedActionsRow();
                    break;
                case MessageKeyboardState.Attachments:
                    DefineAttachmentsRow();
                    break;
            }

        }

        private void DefineMainButtonsProperties(string expandCaption, string actionsCaption, string attachCaption,
                                                 string expandCommand, string actionsCommand, string attachCommand)
        {
            ExpandButton.Text = expandCaption;
            ExpandButton.CallbackData = new CallbackData(_generalCallbackData)
            {
                Command = expandCommand
            };
            ActionsButton.Text = actionsCaption;
            ActionsButton.CallbackData = new CallbackData(_generalCallbackData)
            {
                Command = actionsCommand
            };
            AttachmentsButton.Text = attachCaption;
            AttachmentsButton.CallbackData = new CallbackData(_generalCallbackData)
            {
                Command = attachCommand
            };
        }

        private void DefineMinimizedRow()
        {
            if (!_message.SnippetEqualsBody)
                _firstRow.Add(ExpandButton);
            _firstRow.Add(ActionsButton);
            if (_message.HasAttachments)
                _firstRow.Add(AttachmentsButton);
        }

        private void DefineMaximizedRow()
        {
            _firstRow = PageSliderRow();
            if (!_message.SnippetEqualsBody)
                _secondRow.Add(ExpandButton);
            _secondRow.Add(ActionsButton);
            if (_message.HasAttachments)
                _secondRow.Add(AttachmentsButton);
        }

        private void DefineMinimizedActionsRow()
        {
            if (!_message.SnippetEqualsBody)
                _firstRow.Add(ExpandButton);
            _firstRow.Add(ActionsButton);
            if (_message.HasAttachments)
                _firstRow.Add(AttachmentsButton);
            _secondRow.Add(UnreadButton);
            _secondRow.Add(SpamButton);
            _secondRow.Add(TrashButton);
            _secondRow.Add(ArchiveButton);
            _secondRow.Add(NotifyButton);
        }

        private void DefineMaximizedActionsRow()
        {
            _firstRow = PageSliderRow();
            if (!_message.SnippetEqualsBody)
                _secondRow.Add(ExpandButton);
            _secondRow.Add(ActionsButton);
            if (_message.HasAttachments)
                _secondRow.Add(AttachmentsButton);
            _thirdRow.Add(UnreadButton);
            _thirdRow.Add(SpamButton);
            _thirdRow.Add(TrashButton);
            _thirdRow.Add(ArchiveButton);
            _thirdRow.Add(NotifyButton);
        }

        private void DefineAttachmentsRow()
        {
            var closeButton = new InlineKeyboardButton
            {
                Text = MainButtonCaption.Close,
                CallbackData = new CallbackData(_generalCallbackData)
                {
                    Command = Commands.HIDE_ATTACHMENTS_COMMAND
                }
            };
            var keyboardButtons = new List<InlineKeyboardButton>();
            _message.Attachments.IndexEach((a, i) =>
            {
                keyboardButtons.Add(new InlineKeyboardButton
                {
                    Text = $"{i + 1}. {a.FileName}",
                    CallbackData = new CallbackData(_generalCallbackData)
                    {
                        Command = Commands.GET_ATTACHMENT_COMMAND + $"{i}"
                    }
                });
            });
            keyboardButtons.Add(closeButton);
            _atachmentsKeyboard = keyboardButtons.DivideByLength(4).ToList();
        }

        private List<InlineKeyboardButton> PageSliderRow()
        {
            var row = new List<InlineKeyboardButton>();
            if (_message.MultiPageBody)
            {
                var pageCount = _message.Pages;
                if (_page < pageCount)
                {
                    NextPageButton = new InlineKeyboardButton();
                    NextPageButton.Text = $"To Page {_page + 1} {Emoji.RightArrow}";
                    NextPageButton.CallbackData = new CallbackData(_generalCallbackData)
                    {
                        Command = Commands.NEXTPAGE_COMMAND
                    };
                }
            }
            if (_page > 1)
            {
                PrevPageButton = new InlineKeyboardButton();
                PrevPageButton.Text = $"{Emoji.LeftArrow} To Page {_page - 1}";
                PrevPageButton.CallbackData = new CallbackData(_generalCallbackData)
                {
                    Command = Commands.PREVPAGE_COMMAND
                };
            }
            if (PrevPageButton != null)
                row.Add(PrevPageButton);
            if (NextPageButton != null)
                row.Add(NextPageButton);
            return row;
        }

        private string AttachTextProperty(string label, string trueText, string falseText)
        {
            return _message.LabelIds.Exists(l => l == label)
                ? trueText
                : falseText;
        }

        private string AttachTextProperty(bool state, string trueText, string falseText)
        {
            return state ? trueText : falseText;
        }

        private string AttachCallbackDataProperty(string label, string trueCommand, string falseCommand)
        {
            return _message.LabelIds.Exists(l => l == label)
                ? new CallbackData(_generalCallbackData) { Command = trueCommand }
                : new CallbackData(_generalCallbackData) { Command = falseCommand };
        }

        private string AttachCallbackDataProperty(bool state, string trueCommand, string falseCommand)
        {
            return state
                ? new CallbackData(_generalCallbackData) { Command = trueCommand }
                : new CallbackData(_generalCallbackData) { Command = falseCommand };
        }

        private InlineKeyboardButton InitActionButton(string label, string trueText, string falseText, string trueCommand, string falseCommand)
        {
            return new InlineKeyboardButton
            {
                Text = AttachTextProperty(label, trueText, falseText),
                CallbackData = AttachCallbackDataProperty(label, trueCommand, falseCommand)
            };
        }

        private InlineKeyboardButton InitActionButton(bool state, string trueText, string falseText, string trueCommand, string falseCommand)
        {
            return new InlineKeyboardButton
            {
                Text = AttachTextProperty(state, trueText, falseText),
                CallbackData = AttachCallbackDataProperty(state, trueCommand, falseCommand)
            };
        }

        private readonly FormattedMessage _message;
        private readonly CallbackData _generalCallbackData;
        private readonly MessageKeyboardState _state;
        private readonly bool _isIgnored;
        private int _page;
        private IEnumerable<IEnumerable<InlineKeyboardButton>> _atachmentsKeyboard;


        public InlineKeyboardMarkup InlineKeyboardMarkup { get; private set; }

        private InlineKeyboardButton UnreadButton =>
                InitActionButton(Labels.Unread, ActionButtonCaption.ToRead, ActionButtonCaption.ToUnread,
                           Commands.TO_READ_COMMAND, Commands.TO_UNREAD_COMMAND);

        private InlineKeyboardButton SpamButton =>
            InitActionButton(Labels.Spam, ActionButtonCaption.NotSpam, ActionButtonCaption.Spam,
                       Commands.TO_INBOX_COMMAND, Commands.TO_SPAM_COMMAND);

        private InlineKeyboardButton TrashButton =>
            InitActionButton(Labels.Trash, ActionButtonCaption.Restore, ActionButtonCaption.Delete,
                       Commands.TO_INBOX_COMMAND, Commands.TO_TRASH_COMMAND);

        private InlineKeyboardButton ArchiveButton =>
            InitActionButton(Labels.Inbox, ActionButtonCaption.ToArchive, ActionButtonCaption.ToInbox,
                       Commands.ARCHIVE_COMMAND, Commands.TO_INBOX_COMMAND);

        private InlineKeyboardButton NotifyButton =>
            InitActionButton(_isIgnored, ActionButtonCaption.Unignore, ActionButtonCaption.Ignore, Commands.UNIGNORE_COMMAND,
                Commands.IGNORE_COMMAND);


        private InlineKeyboardButton ExpandButton { get; } = new InlineKeyboardButton();
        private InlineKeyboardButton ActionsButton { get; } = new InlineKeyboardButton();
        private InlineKeyboardButton AttachmentsButton { get; } = new InlineKeyboardButton();

        private InlineKeyboardButton NextPageButton { get; set; }
        private InlineKeyboardButton PrevPageButton { get; set; }

        private List<InlineKeyboardButton> _firstRow = new List<InlineKeyboardButton>();
        private List<InlineKeyboardButton> _secondRow = new List<InlineKeyboardButton>();
        private List<InlineKeyboardButton> _thirdRow = new List<InlineKeyboardButton>();

        internal static class Labels
        {
            public static string Unread => "UNREAD";
            public static string Spam => "SPAM";
            public static string Trash => "TRASH";
            public static string Inbox => "INBOX";
        }

        internal static class ActionButtsonCaption
        {
            public static string ToRead => $"{Emoji.Eye} To Read";
            public static string ToUnread => $"{Emoji.RedArrowedEnvelope} To Unread";
            public static string NotSpam => $"{Emoji.HeartEnvelope} Not Spam";
            public static string Spam => $"{Emoji.RestrictionSign} Spam";
            public static string Restore => $"{Emoji.ClosedMailbox} Restore";
            public static string Delete => $"{Emoji.RecycleBin} Delete";
            public static string ToArchive => $"{Emoji.Multifolder} To Archive";
            public static string ToInbox => $"{Emoji.ClosedMailbox} To Inbox";
            public static string Unignore => "Unignore";
            public static string Ignore => "Ignore";
        }

        internal static class MainButtonCaption
        {
            public static string Expand => $"{Emoji.DownTriangle}Expand";
            public static string Actions => $"{Emoji.TurnedDownArrow} Actions";
            public static string Attachments => $"{Emoji.OpenFileFolder}Attachments";
            public static string Hide => $"{Emoji.UpTriangle} Hide";
            public static string PressedActions => $"{Emoji.TurnedUpArrow} Actions";
            public static string Close => "Close";

        }
    }
}