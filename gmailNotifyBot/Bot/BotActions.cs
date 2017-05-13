using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
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
            var button = new InlineKeyboardButton
            {
                Text = "Reauthorize",
                CallbackData = new CallbackData
                {
                    Command = Commands.CONNECT_COMMAND
                }
            };
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>> { new List<InlineKeyboardButton> { button } }
            };
            await _telegramMethods.SendMessageAsync(userId, @"I am lost your credentials. Please reauthorize me using /connect command or click a button below.", null, false, false, null, keyboard);
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

        public async Task AuthorizationErrorMessage(string userId)
        {
            await
                _telegramMethods.SendMessageAsync(userId, "I can't send you the authorization link, I'm so sorry!");
        }

        public async Task EmailAddressMessage(string userId, string emailAddress)
        {
            await _telegramMethods.SendMessageAsync(userId, emailAddress);
        }

        public async Task EmptyLabelMessage(string userId, string labelId, int page)
        {
            await _telegramMethods.SendMessageAsync(userId, $"{Emoji.Denied}  There is no messages in your {labelId} on page {page}.");
        }

        public async Task EmptyAllMessage(string userId, int page)
        {
            await _telegramMethods.SendMessageAsync(userId, $"{Emoji.Denied}  There is no messages in All Mail on page {page}.");
        }

        public async Task GmailInlineCommandMessage(string userId)
        {
            await _telegramMethods.SendMessageAsync(userId, $"@{_botSettings.Username} Inbox:");
        }


        public async Task ShowShortMessageAnswerInlineQuery(string inlineQueryId, List<FormattedMessage> messages, int? offset = null)
        {
            var inlineQueryResults = new List<InlineQueryResult>();
            foreach (var message in messages)
            {
                inlineQueryResults.Add(new InlineQueryResultArticle
                {
                    Id = message.Id,
                    Title = $"From: {message.SenderName} <{message.Date}> /r/n {message.Subject}",
                    Description = message.Snippet,
                    InputMessageContent = new InputTextMessageContent
                    {
                        MessageText = "Message:"
                    }
                });
            }
            if (!offset.HasValue)
                await _telegramMethods.AnswerInlineQueryAsync(inlineQueryId, inlineQueryResults, 30, true);
            else
                await _telegramMethods.AnswerInlineQueryAsync(inlineQueryId, inlineQueryResults, 30, true, offset.ToString());
        }

        public async Task EditProceedMessage(string chatId, string messageId)
        {
            await _telegramMethods.EditMessageTextAsync("Success", chatId, messageId);
        }

        public async Task ChosenShortMessage(string chatId, FormattedMessage formattedMessage, bool isIgnored)
        {
            var header = formattedMessage.Header;
            var message = header + $"\r\n\r\n {formattedMessage.Snippet}";
            var keyboard = MessageKeyboardMarkup(formattedMessage, 0, MessageKeyboardState.Minimized, isIgnored);
            await _telegramMethods.SendMessageAsync(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public async Task UpdateMessage(string chatId, int messageId, FormattedMessage formattedMessage, int page, MessageKeyboardState state, bool isIgnored)
        {
            var header = formattedMessage.Header;
            var keyboard = MessageKeyboardMarkup(formattedMessage, page, state, isIgnored);
            var displayedMessage = page == 0
                ? header + $"\r\n\r\n{formattedMessage.Snippet}"
                : header + $"\r\n\r\n{formattedMessage.DesirableBody[page - 1]}";
            await _telegramMethods.EditMessageTextAsync(displayedMessage, chatId, messageId.ToString(), null, ParseMode.Html, null, keyboard);
        }

        private InlineKeyboardMarkup MessageKeyboardMarkup(FormattedMessage message, int page, MessageKeyboardState state, bool isIgnored)
        {
            message.NullInspect(nameof(message));

            var keyboardMarkup = new InlineKeyboardMarkup { InlineKeyboard = new List<List<InlineKeyboardButton>>() };
            var firstRow = new List<InlineKeyboardButton>();
            var secondRow = new List<InlineKeyboardButton>();
            var thirdRow = new List<InlineKeyboardButton>();
            var expandButton = new InlineKeyboardButton();
            var actionsButton = new InlineKeyboardButton();
            var generalCallbackData = new CallbackData
            {
                MessageId = message.Id,
                Page = page,
                MessageKeyboardState = state,
                Etag = message.ETag
            };
            InlineKeyboardButton nextPageButton = null;
            InlineKeyboardButton prevPageButton = null;
            var unreadButton = new InlineKeyboardButton
            {
                Text = message.LabelIds.Exists(label => label == "UNREAD")
                ? $"{Emoji.Eye} To Read"
                : $"{Emoji.RedArrowedMail} To Unread",
                CallbackData = message.LabelIds.Exists(label => label == "UNREAD")
                ? new CallbackData(generalCallbackData) { Command = Commands.TO_READ_COMMAND }
                : new CallbackData(generalCallbackData) { Command = Commands.TO_UNREAD_COMMAND }
            };
            var spamButton = new InlineKeyboardButton
            {
                Text = message.LabelIds.Exists(label => label == "SPAM")
                ? $"{Emoji.LovelyLatter} Not Spam"
                : $"{Emoji.RestrictionSign} Spam",
                CallbackData = message.LabelIds.Exists(label => label == "SPAM")
                ? new CallbackData(generalCallbackData) { Command = Commands.TO_INBOX_COMMAND }
                : new CallbackData(generalCallbackData) { Command = Commands.TO_SPAM_COMMAND }
            };
            var trashButton = new InlineKeyboardButton
            {
                Text = message.LabelIds.Exists(label => label == "TRASH")
                ? $"{Emoji.ClosedMailbox} Restore"
                : $"{Emoji.RecycleBin} Delete",
                CallbackData = message.LabelIds.Exists(label => label == "TRASH")
                ? new CallbackData(generalCallbackData) { Command = Commands.TO_INBOX_COMMAND }
                : new CallbackData(generalCallbackData) { Command = Commands.TO_TRASHCOMMAND }
            };
            var archiveButton = new InlineKeyboardButton
            {
                Text = message.LabelIds.Exists(label => label == "INBOX")
                ? $"{Emoji.Multifolder} To Archive"
                : $"{Emoji.ClosedMailbox} To Inbox",
                CallbackData = message.LabelIds.Exists(label => label == "INBOX")
                ? new CallbackData(generalCallbackData) { Command = Commands.ARCHIVE_COMMAND }
                : new CallbackData(generalCallbackData) { Command = Commands.TO_INBOX_COMMAND }
            };
            var notifyButton = new InlineKeyboardButton
            {
                Text = isIgnored ? "Unignore" : "Ignore",
                CallbackData = isIgnored
                ? new CallbackData(generalCallbackData) { Command = Commands.UNIGNORE_COMMAND }
                : new CallbackData(generalCallbackData) { Command = Commands.IGNORE_COMMAND }
            };

            var pageSliderFunc = new Func<List<InlineKeyboardButton>>(() =>
            {
                var row = new List<InlineKeyboardButton>();
                if (message.MultiPageBody)
                {
                    var pageCount = message.Pages;
                    if (page < pageCount)
                    {
                        nextPageButton = new InlineKeyboardButton();
                        nextPageButton.Text = $"To Page {page + 1} {Emoji.RightArrow}";
                        nextPageButton.CallbackData = new CallbackData(generalCallbackData)
                        {
                            Command = Commands.NEXTPAGE_COMMAND
                        };
                    }
                }
                if (page > 1)
                {
                    prevPageButton = new InlineKeyboardButton();
                    prevPageButton.Text = $"{Emoji.LeftArrow} To Page {page - 1}";
                    prevPageButton.CallbackData = new CallbackData(generalCallbackData)
                    {
                        Command = Commands.PREVPAGE_COMMAND
                    };
                }
                if (prevPageButton != null)
                    row.Add(prevPageButton);
                if (nextPageButton != null)
                    row.Add(nextPageButton);
                return row;
            });
            string expandButtonCommand = "";
            string actionsButtonCommand = "";
            switch (state)
            {
                case MessageKeyboardState.Minimized:
                    #region minimized

                    expandButton.Text = $"{Emoji.DownTriangle} Expand";
                    expandButtonCommand = Commands.EXPAND_COMMAND;
                    actionsButton.Text = $"{Emoji.TurnedDownArrow} Actions";
                    actionsButtonCommand = Commands.EXPAND_ACTIONS_COMMAND;
                    if (!message.SnippetEqualsBody)
                        firstRow.Add(expandButton);
                    firstRow.Add(actionsButton);
                    break;
                #endregion
                case MessageKeyboardState.Maximized:
                    #region maximized
                    expandButton.Text = $"{Emoji.UpTriangle} Hide";
                    expandButtonCommand = Commands.HIDE_COMMAND;
                    actionsButton.Text = $"{Emoji.TurnedDownArrow} Actions";
                    actionsButtonCommand = Commands.EXPAND_ACTIONS_COMMAND;
                    firstRow = pageSliderFunc();
                    if (!message.SnippetEqualsBody)
                        secondRow.Add(expandButton);
                    secondRow.Add(actionsButton);
                    break;
                #endregion
                case MessageKeyboardState.MinimizedActions:
                    #region minimazedActions
                    expandButton.Text = $"{Emoji.DownTriangle} Expand";
                    expandButtonCommand = Commands.EXPAND_COMMAND;
                    actionsButton.Text = $"{Emoji.TurnedUpArrow} Actions";
                    actionsButtonCommand = Commands.HIDE_ACTIONS_COMMAND;
                    if (!message.SnippetEqualsBody)
                        firstRow.Add(expandButton);
                    firstRow.Add(actionsButton);
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
                    expandButtonCommand = Commands.HIDE_COMMAND;
                    actionsButton.Text = $"{Emoji.TurnedUpArrow} Actions";
                    actionsButtonCommand = Commands.HIDE_ACTIONS_COMMAND;
                    firstRow = pageSliderFunc();
                    if (!message.SnippetEqualsBody)
                        secondRow.Add(expandButton);
                    secondRow.Add(actionsButton);
                    thirdRow.Add(unreadButton);
                    thirdRow.Add(spamButton);
                    thirdRow.Add(trashButton);
                    thirdRow.Add(archiveButton);
                    thirdRow.Add(notifyButton);
                    break;
                    #endregion
            }
            expandButton.CallbackData = new CallbackData(generalCallbackData)
            {
                Command = expandButtonCommand
            };
            actionsButton.CallbackData = new CallbackData(generalCallbackData)
            {
                Command = actionsButtonCommand
            };
            if (firstRow.Count > 0)
                keyboardMarkup.InlineKeyboard.Add(firstRow);
            if (secondRow.Count > 0)
                keyboardMarkup.InlineKeyboard.Add(secondRow);
            if (thirdRow.Count > 0)
                keyboardMarkup.InlineKeyboard.Add(thirdRow);
            return keyboardMarkup;
        }


        private readonly TelegramMethods _telegramMethods;
        private readonly BotSettings _botSettings;
    }

    public enum MessageKeyboardState
    {
        [EnumMember(Value = "minimized")]
        Minimized,
        [EnumMember(Value = "maximized")]
        Maximized,
        [EnumMember(Value = "minimizedActions")]
        MinimizedActions,
        [EnumMember(Value = "maximizedActions")]
        MaximizedActions
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