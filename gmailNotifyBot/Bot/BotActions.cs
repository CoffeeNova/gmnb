using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult;
using CoffeeJelly.TelegramBotApiWrapper.Types.InputMessageContent;
using TelegramMethods = CoffeeJelly.TelegramBotApiWrapper.Methods.TelegramMethods;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    internal class BotActions
    {
        public BotActions(string token)
        {
            _botSettings = BotSettings.Instance;
            Debug.Assert(_botSettings != null && _botSettings.AllSettingsAreSet(),
                "Set all properties at BotSettings class.");
            _telegramMethods = new TelegramMethods(token);
        }

        public async Task WrongCredentialsMessage(string userId)
        {
            await _telegramMethods.SendMessageAsync(userId, @"I am lost your credentials. Please reauthorize me using /connect command or click a button below.", null, false, false, null, ReauthorizeButton);
        }

        private static InlineKeyboardMarkup ReauthorizeButton
        {
            get
            {
                var reauthorizeButton = new InlineKeyboardButton
                {
                    Text = "Reauthorize",
                    CallbackData = Commands.CONNECT_COMMAND
                };

                return new InlineKeyboardMarkup
                {
                    InlineKeyboard = new List<List<InlineKeyboardButton>>
                    {
                        new List<InlineKeyboardButton>
                        {
                            reauthorizeButton
                        }
                    }
                };
            }
        }

        public async Task AuthorizeMessage(string userId, Uri notifyAccessUri, Uri fullAccessUri)
        {
            var notifyAccessButton = new InlineKeyboardButton
            {
                Text = "Mail Notify",
                Url = notifyAccessUri.OriginalString
            };
            var fullAccessButton = new InlineKeyboardButton
            {
                Text = "Full Access",
                Url = fullAccessUri.OriginalString
            };
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton>
                    {
                        notifyAccessButton,
                        fullAccessButton
                    }
                }
            };
            await _telegramMethods.SendMessageAsync(userId,
                $"Open one of this link to authorize the bot to get: ", null, false, false, null, keyboard);
        }

        public async Task AuthorizationTimeExpiredMessage(string userId)
        {
            await
                _telegramMethods.SendMessageAsync(userId,
                    @"Time for authorization has expired. Please type again /connect command.");
        }

        public async Task AuthorizationFailedMessage(string userId)
        {
            await
                _telegramMethods.SendMessageAsync(userId, "Authorization failed. See ya!");
        }

        public async Task AuthorizationSuccessfulMessage(string userId)
        {
            await
                _telegramMethods.SendMessageAsync(userId, "Authorization successful! Now you can recieve notifications about new emails and use other functions!");
        }

        public async Task EmailAddressMessage(string userId, string emailAddress)
        {
            await _telegramMethods.SendMessageAsync(userId, emailAddress);
        }

        public async Task EmptyInboxMessage(string userId)
        {
            await _telegramMethods.SendMessageAsync(userId, $"{Emoji.Denied}  There is no messages in your Inbox.");
        }

        public async Task GmailInlineCommandMessage(string userId)
        {
            await _telegramMethods.SendMessageAsync(userId, $"@{_botSettings.Username} Inbox:");
        }

        public async Task ShowShortMessageAnswerInlineQuery(string inlineQueryId, List<FormattedGmailMessage> messages)
        {
            var inlineQueryResults = new List<InlineQueryResult>();
            foreach (var message in messages)
            {
                inlineQueryResults.Add(new InlineQueryResultArticle
                {
                    Id = message.Id,//Base64.Encode("Inbox:" + message.Id),
                    Title = $"From: {message.SenderName} <{message.Date}> /r/n {message.Subject}",
                    Description = message.Snippet,
                    InputMessageContent = new InputTextMessageContent
                    {
                        MessageText = "Message:"
                    }
                });
            }
            await _telegramMethods.AnswerInlineQueryAsync(inlineQueryId, inlineQueryResults, 30, true, "5");
        }

        public async Task EditProceedMessage(string chatId, string messageId)
        {
            await _telegramMethods.EditMessageTextAsync("Success", chatId, messageId);
        }

        public async Task ChosenShortMessage(string userId, FormattedGmailMessage message, int page, MessageKeyboardState state, bool isIgnored)
        {
            var header = MarkdownStyledMessageHeader(message);
            var keyboard = MessageKeyboardMarkup(userId, message, page, state, isIgnored);
            await _telegramMethods.SendMessageAsync(userId, header, ParseMode.Markdown, false, false, null, keyboard);
        }

        private string MarkdownStyledMessageHeader(FormattedGmailMessage message)
        {
            return
                $"From: *{message.SenderName}*   _{message.SenderEmail}_\r\n *{message.Subject}* \r\n\r\n {message.Snippet}";
        }

        private InlineKeyboardMarkup MessageKeyboardMarkup(string userid, FormattedGmailMessage message, int page, MessageKeyboardState state, bool isIgnored)
        {
            message.NullInspect(nameof(message));

            var keyboardMarkup = new InlineKeyboardMarkup();
            keyboardMarkup.InlineKeyboard = new List<List<InlineKeyboardButton>>();
            var firstRow = new List<InlineKeyboardButton>();
            var secondRow = new List<InlineKeyboardButton>();
            var thirdRow = new List<InlineKeyboardButton>();
            var expandButton = new InlineKeyboardButton();
            var actions = new InlineKeyboardButton();
            var showInBrowserButton = new InlineKeyboardButton
            {
                Text = "Open in Browser",
                Url = $@"https://mail.google.com/mail/u/0/#inbox/{message.Id}"
            };
            var nextPageButton = new InlineKeyboardButton();
            var prevPageButton = new InlineKeyboardButton();
            var unreadButton = new InlineKeyboardButton
            {
                Text = message.LabelIds.Exists(label => label == "UNREAD")
                ? $"{Emoji.Eye} To Read"
                : $"{Emoji.RedArrowedMail} To Unread",
                CallbackData = message.LabelIds.Exists(label => label == "UNREAD")
                ? Commands.TO_READ_COMMAND
                : Commands.TO_UNREAD_COMMAND
            };
            var spamButton = new InlineKeyboardButton
            {
                Text = message.LabelIds.Exists(label => label == "SPAM")
                ? $"{Emoji.LovelyLatter} Not Spam"
                : $"{Emoji.RestrictionSign} Spam",
                CallbackData = message.LabelIds.Exists(label => label == "SPAM")
                ? Commands.REMOVE_SPAM_COMMAND
                : Commands.TO_SPAM_COMMAND
            };
            var trashButton = new InlineKeyboardButton
            {
                Text = message.LabelIds.Exists(label => label == "TRASH")
                ? $"{Emoji.ClosedMailbox} Restore"
                : $"{Emoji.RecycleBin} Delete",
                CallbackData = message.LabelIds.Exists(label => label == "TRASH")
                ? Commands.TO_INBOX_COMMAND
                : Commands.DELETE_COMMAND
            };
            var archiveButton = new InlineKeyboardButton
            {
                Text = message.LabelIds.Exists(label => label == "INBOX")
                ? $"{Emoji.Multifolder} To Archive"
                : $"{Emoji.ClosedMailbox} To Inbox",
                CallbackData = message.LabelIds.Exists(label => label == "INBOX")
                ? Commands.ARCHIVE_COMMAND
                : Commands.TO_INBOX_COMMAND
            };
            var notifyButton = new InlineKeyboardButton
            {
                Text = isIgnored ? "Unignore" : "Ignore",
                CallbackData = isIgnored ? Commands.UNIGNORE_COMMAND : Commands.IGNORE_COMMAND
            };

            var pageSliderFunc = new Func<List<InlineKeyboardButton>>(() =>
            {
                var row = new List<InlineKeyboardButton>();
                if (message.MultiPartBody)
                {
                    if (page < message.Body?.Count)
                    {
                        nextPageButton.Text = $"To {page + 1} Page {Emoji.RightArrow}";
                        nextPageButton.CallbackData = $"{Commands.NEXTPAGE_COMMAND} {page + 1}";
                    }
                    if (page > 1)
                    {
                        prevPageButton.Text = $"{Emoji.LeftArrow} To {page - 1} Page";
                        prevPageButton.CallbackData = $"{Commands.PREVPAGE_COMMAND} {page - 1}";
                    }
                    row.Add(prevPageButton);
                    row.Add(nextPageButton);
                }
                return row;
            });
            switch (state)
            {
                case MessageKeyboardState.Minimized:
                    #region minimized

                    expandButton.Text = $"{Emoji.DownTriangle} Expand";
                    expandButton.CallbackData = Commands.EXPANDCOMMAND;
                    actions.Text = $"{Emoji.Dashboard} Actions";
                    actions.CallbackData = Commands.EXPANDMESSAGEACTIONS;
                    firstRow.Add(expandButton);
                    firstRow.Add(actions);
                    firstRow.Add(showInBrowserButton);

                    break;
                #endregion
                case MessageKeyboardState.Maximized:
                    #region maximized
                    expandButton.Text = $"{Emoji.UpTriangle} Hide";
                    expandButton.CallbackData = Commands.HIDECOMMAND;
                    actions.Text = $"{Emoji.TurnedDownArrow} Actions";
                    actions.CallbackData = Commands.EXPANDMESSAGEACTIONS;
                    firstRow = pageSliderFunc();
                    secondRow.Add(expandButton);
                    secondRow.Add(actions);
                    secondRow.Add(showInBrowserButton);
                    break;
                #endregion
                case MessageKeyboardState.MinimizedActions:
                    #region minimazedActions
                    expandButton.Text = $"{Emoji.DownTriangle} Expand";
                    expandButton.CallbackData = Commands.EXPANDCOMMAND;
                    actions.Text = $"{Emoji.TurnedUpArrow} Actions";
                    actions.CallbackData = Commands.EXPANDMESSAGEACTIONS;
                    firstRow.Add(expandButton);
                    firstRow.Add(actions);
                    firstRow.Add(showInBrowserButton);
                    secondRow.Add(unreadButton);
                    secondRow.Add(spamButton);
                    secondRow.Add(trashButton);
                    secondRow.Add(archiveButton);
                    secondRow.Add(notifyButton);
                    break;
                #endregion
                case MessageKeyboardState.MaximizedActions:
                    #region maximizedActions
                    expandButton.Text = $"{Emoji.UpTriangle} Hide";
                    expandButton.CallbackData = Commands.HIDECOMMAND;
                    actions.Text = $"{Emoji.TurnedDownArrow} Actions";
                    actions.CallbackData = Commands.EXPANDMESSAGEACTIONS;
                    firstRow = pageSliderFunc();
                    secondRow.Add(expandButton);
                    secondRow.Add(actions);
                    secondRow.Add(showInBrowserButton);
                    thirdRow.Add(unreadButton);
                    thirdRow.Add(spamButton);
                    thirdRow.Add(trashButton);
                    thirdRow.Add(archiveButton);
                    thirdRow.Add(notifyButton);
                    break;
                    #endregion
            }
            keyboardMarkup.InlineKeyboard.Add(firstRow);
            keyboardMarkup.InlineKeyboard.Add(secondRow);
            keyboardMarkup.InlineKeyboard.Add(thirdRow);
            return keyboardMarkup;
        }

        public enum MessageKeyboardState
        {
            Minimized,
            Maximized,
            MinimizedActions,
            MaximizedActions
        }
        private readonly TelegramMethods _telegramMethods;
        private BotSettings _botSettings;
    }



    public static class Emoji
    {
        public const string Denied = "\ud83d\udeab"; //red crossed-out circle
        public const string DownArrow = "\u2b07\ufe0f"; //white down facing arrow in a blue rectangle
        public const string DownTriangle = "\ud83d\udd3d"; //white down triangle in a blue rectangle
        public const string UpTriangle = "\ud83d\udd3c"; //white up triangle in a blue rectangle
        public const string Dashboard = "\ud83c\udf9b";
        public const string RightArrow = "\u27a1\ufe0f"; //white right facing arrow in a blue rectangle
        public const string LeftArrow = "\u2b05\ufe0f"; //white left facing arrow in a blue rectangle
        public const string TurnedUpArrow = "\u2934\ufe0f"; //while turned up from left arrow in a blue rectange
        public const string TurnedDownArrow = "\u2935\ufe0f"; //while turned down from left arrow in a blue rectange
        public const string Eye = "\ud83d\udc41";
        public const string RedArrowedMail = "\ud83d\udce9"; //envelope with red arrow
        public const string LovelyLatter = "\ud83d\udc8c"; //envelope with red heart
        public const string RestrictionSign = "\u26d4\ufe0f"; //red restriction sign
        public const string RecycleBin = "\ud83d\uddd1";
        public const string ClosedMailbox = "\ud83d\udcea"; // closed blue mailbox
        public const string Multifolder = "\ud83d\uddc2";
    }

}