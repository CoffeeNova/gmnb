using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper;
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
            _telegramMethods = new TelegramMethods(token);
#if DEBUG
            _contactsThumbUrl = @"https://pbs.twimg.com/media/DAf2gvDXcAAxk0w.jpg";
#else
            //_contactsThumbUrl = $@"https://{Settings.DomainName}/{Settings.ImagesPath}silhouette48.jpg";
            _contactsThumbUrl = $@"https://{Settings.DomainName}/Images/Silhouette48";
#endif
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
                _telegramMethods.SendMessageAsync(userId, "Authorization successful! Now you can receive notifications about new emails and use other functions!");
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
            await _telegramMethods.SendMessageAsync(userId, $"@{Settings.BotName} Inbox:");
        }


        public async Task ShowShortMessageAnswerInlineQuery(string inlineQueryId, List<FormattedMessage> messages, int? offset = null)
        {
            var inlineQueryResults = new List<InlineQueryResult>();
            foreach (var message in messages)
            {
                var date = message.Date.Date == DateTime.Now.Date
                    ? message.Date.ToString("HH:mm")
                    : message.Date.ToString("dd.MM.yy");
                inlineQueryResults.Add(new InlineQueryResultArticle
                {
                    Id = message.Id,
                    Title = ShortMessageTitleFormatter(message.From.Name, message.From.Email, date),
                    Description = message.Subject,
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

        private string ShortMessageTitleFormatter(string senderName, string senderEmail, string date)
        {
            const int maxLine = 44;

            var builder = new StringBuilder(maxLine);
            builder.Append(date);
            builder.Append(' ');
            builder.Append(senderName);
            if (maxLine - builder.Length > senderEmail.Length + 2)
                builder.Append($" /{senderEmail}/");
            return builder.ToString();
        }

        public async Task EditProceedMessage(string chatId, string messageId)
        {
            await _telegramMethods.EditMessageTextAsync("Success", chatId, messageId);
        }

        public async Task ShowShortMessageAsync(string chatId, FormattedMessage formattedMessage, bool isIgnored)
        {
            var header = formattedMessage.Header;
            var message = Emoji.ClosedEmailEnvelop + header + $"{Environment.NewLine}{Environment.NewLine} {formattedMessage.Snippet}";
            var keyboard = ReceivedMessageKeyboardMarkup(formattedMessage, 0, MessageKeyboardState.Minimized, isIgnored);
            await _telegramMethods.SendMessageAsync(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public void ShowShortMessage(string chatId, FormattedMessage formattedMessage, bool isIgnored)
        {
            var header = formattedMessage.Header;
            var message = Emoji.ClosedEmailEnvelop + header + $"{Environment.NewLine}{Environment.NewLine} {formattedMessage.Snippet}";
            var keyboard = ReceivedMessageKeyboardMarkup(formattedMessage, 0, MessageKeyboardState.Minimized, isIgnored);
            _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public async Task UpdateMessage(string chatId, int messageId, FormattedMessage formattedMessage, int page, MessageKeyboardState state, bool isIgnored)
        {
            var header = formattedMessage.Header;
            var keyboard = ReceivedMessageKeyboardMarkup(formattedMessage, page, state, isIgnored);
            var displayedMessage = page == 0
                ? Emoji.ClosedEmailEnvelop + header + $"{Environment.NewLine}{Environment.NewLine}{formattedMessage.Snippet}"
                : Emoji.RedArrowedEnvelope + header + $"{Environment.NewLine}{Environment.NewLine}{formattedMessage.DesirableBody[page - 1]}";
            await _telegramMethods.EditMessageTextAsync(displayedMessage, chatId, messageId.ToString(), null, ParseMode.Html, null, keyboard);
        }

        public async Task SpecifyNewMailMessage(string chatId)
        {
            var keyboard = NewMessageInlineKeyboardMarkup();
            await _telegramMethods.SendMessageAsync(chatId, _newMessageText, ParseMode.Html, false, false, null, keyboard);
        }

        public async Task SendAttachmentsListMessage(string chatId, FormattedMessage formattedMessage)
        {
            formattedMessage.NullInspect(nameof(formattedMessage));
            if (!formattedMessage.HasAttachments)
                throw new InvalidOperationException($"{nameof(formattedMessage.HasAttachments)} property must equals true to avoid this exception.");

            var keyboard = AttachmentsReplyKeyboardMarkup(formattedMessage);
            var messageTextBuilder = new StringBuilder($"Files attached to this message:{Environment.NewLine}");
            formattedMessage.Attachments.IndexEach((a, i) =>
            {
                messageTextBuilder.AppendLine($"{i + 1}. {a.FileName}");
            });
            messageTextBuilder.AppendLine();
            messageTextBuilder.Append("Please select a file by number to download it:");
            await _telegramMethods.SendMessageAsync(chatId, messageTextBuilder.ToString(), null, false, false, null, keyboard);
        }

        public async Task ShowContactsAnswerInlineQuery(string inlineQueryId, IEnumerable<UserInfo> contacts, int? offset = null)
        {
            var inlineQueryResults = new List<InlineQueryResult>();
            foreach (var contact in contacts)
            {
                inlineQueryResults.Add(new InlineQueryResultArticle
                {
                    Id = contact.Email,
                    Title = contact.Email,
                    Description = contact.Name,
                    InputMessageContent = new InputTextMessageContent
                    {
                        MessageText = $"Recipient added:{Environment.NewLine}{contact.Name} <{contact.Email}>"
                    },
                    ThumbUrl = _contactsThumbUrl,
                    ThumbHeight = 48,
                    ThumbWidth = 48
                });
            }
            if (!offset.HasValue)
                await _telegramMethods.AnswerInlineQueryAsync(inlineQueryId, inlineQueryResults, 30, true);
            else
                await _telegramMethods.AnswerInlineQueryAsync(inlineQueryId, inlineQueryResults, 30, true, offset.ToString());
        }


        public async Task UpdateNewMailMessage(string chatId, int messageId, List<string> recipients, bool isIgnored)
        {
            var keyboard = NewMessageInlineKeyboardMarkup();
            await _telegramMethods.EditMessageTextAsync(_newMessageText, chatId, messageId.ToString(), null, ParseMode.Html, null, keyboard);
        }

        private ReplyKeyboardMarkup RecipientsReplyKeyboardMarkup(string recepient)
        {
            var keyboardMarkup = new ReplyKeyboardMarkup();
            var testButton = new KeyboardButton
            {
                Text = "TEST BUTTON"
            };
            var firstRow = new List<KeyboardButton> { testButton };
            var keyboard = new List<List<KeyboardButton>> { firstRow };
            keyboardMarkup.Keyboard = keyboard;

            return keyboardMarkup;
        }

        private ReplyKeyboardMarkup AttachmentsReplyKeyboardMarkup(FormattedMessage message)
        {
            var closeButton = new KeyboardButton
            {
                Text = "Close"
            };
            var keyboardButtons = new List<KeyboardButton>();
            message.Attachments.IndexEach((a, i) =>
            {
                keyboardButtons.Add(new KeyboardButton {Text = i + 1.ToString()});
            });
            var keyboard = keyboardButtons.DivideByLength(5).ToList();
            keyboard.Add(new List<KeyboardButton> { closeButton });

            return new ReplyKeyboardMarkup { Keyboard = keyboard , ResizeKeyboard = true};
        }

        private InlineKeyboardMarkup NewMessageInlineKeyboardMarkup()
        {
            var recipientsButton = new InlineKeyboardButton
            {
                Text = $"{Emoji.BoyAndGirl}Add Recipients",
                SwitchInlineQueryCurrentChat = Commands.RECIPIENTS_INLINE_QUERY_COMMAND
            };
            var subjectButton = new InlineKeyboardButton
            {
                Text = $"{Emoji.AbcLowerCase}Subject",
                CallbackData = new CallbackData
                {
                    Command = Commands.ADD_SUBJECT_COMMAND
                }
            };
            var messageButton = new InlineKeyboardButton
            {
                Text = $"{Emoji.M}Message",
                CallbackData = new CallbackData
                {
                    Command = Commands.ADD_TEXT_MESSAGE_COMMAND
                }
            };
            var ccButtons = new InlineKeyboardButton
            {
                Text = $"{Emoji.BoyAndGirl}CC Recipients",
                CallbackData = new CallbackData
                {
                    Command = Commands.CC_RECIPIENTS_MESSAGE_COMMAND
                }
            };
            var bccButtons = new InlineKeyboardButton
            {
                Text = $"{Emoji.MaleFemaleShadows}BCC Recipients",
                CallbackData = new CallbackData
                {
                    Command = Commands.BCC_RECIPIENTS_MESSAGE_COMMAND
                }
            };
            var firstRow = new List<InlineKeyboardButton>
            {
                recipientsButton,
                subjectButton,
                messageButton
            };
            var secondRow = new List<InlineKeyboardButton>
            {
                ccButtons,
                bccButtons
            };
            var inlineKeyboard = new List<List<InlineKeyboardButton>> { firstRow, secondRow };

            return new InlineKeyboardMarkup { InlineKeyboard = inlineKeyboard };
        }

        private InlineKeyboardMarkup ReceivedMessageKeyboardMarkup(FormattedMessage message, int page, MessageKeyboardState state, bool isIgnored)
        {
            message.NullInspect(nameof(message));

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
            };
            var attachmentsButton = new InlineKeyboardButton
            {
                Text = $"{Emoji.OpenFileFilder}Attachments",
                CallbackData = new CallbackData(generalCallbackData)
                {
                    Command = Commands.GET_ATTACHMENTS_COMMAND,
                }
            };
            InlineKeyboardButton nextPageButton = null;
            InlineKeyboardButton prevPageButton = null;
            var unreadButton = new InlineKeyboardButton
            {
                Text = message.LabelIds.Exists(label => label == "UNREAD")
                ? $"{Emoji.Eye} To Read"
                : $"{Emoji.RedArrowedEnvelope} To Unread",
                CallbackData = message.LabelIds.Exists(label => label == "UNREAD")
                ? new CallbackData(generalCallbackData) { Command = Commands.TO_READ_COMMAND }
                : new CallbackData(generalCallbackData) { Command = Commands.TO_UNREAD_COMMAND }
            };
            var spamButton = new InlineKeyboardButton
            {
                Text = message.LabelIds.Exists(label => label == "SPAM")
                ? $"{Emoji.HeartEnvelope} Not Spam"
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
                    if (message.HasAttachments)
                        firstRow.Add(attachmentsButton);
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
                    if (message.HasAttachments)
                        secondRow.Add(attachmentsButton);
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
                    if (message.HasAttachments)
                        firstRow.Add(attachmentsButton);
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
                    if (message.HasAttachments)
                        secondRow.Add(attachmentsButton);
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
            var inlineKeyboard = new List<List<InlineKeyboardButton>>();
            if (firstRow.Count > 0)
                inlineKeyboard.Add(firstRow);
            if (secondRow.Count > 0)
                inlineKeyboard.Add(secondRow);
            if (thirdRow.Count > 0)
                inlineKeyboard.Add(thirdRow);

            return new InlineKeyboardMarkup { InlineKeyboard = inlineKeyboard };
        }


        private readonly TelegramMethods _telegramMethods;
        private readonly string _newMessageText =
                        $"{Emoji.New} Please specify the <b>Recipients</b>, a <b>Subject</b> and the <b>Content</b> of the email: " +
                        $"{Environment.NewLine}{Emoji.InfoSign} You can use quick command, just type in the chat:" +
                        $"{Environment.NewLine}<i>/new \"recipient1@gmail.com, recipient2@gmail.com,...\" \"subject\" \"email text\"</i>" +
                        $"{Environment.NewLine}and press Enter to quick send the email." +
                        $"{Environment.NewLine}{Emoji.InfoSign} For multiple recipients use comma separator.";

        private BotSettings Settings = BotInitializer.Instance.BotSettings;
        private string _contactsThumbUrl;
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
}