using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult;
using CoffeeJelly.TelegramBotApiWrapper.Types.InputMessageContent;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using File = CoffeeJelly.TelegramBotApiWrapper.Types.General.File;
using TelegramMethods = CoffeeJelly.TelegramBotApiWrapper.Methods.TelegramMethods;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity
{
    internal class BotActions
    {
        public BotActions(string token)
        {
            token.NullInspect(nameof(token));
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
                CallbackData = new GetCallbackData
                {
                    Command = TextCommand.AUTHORIZE_COMMAND
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

        public async Task EmptyLabelMessage(string userId, string labelId)
        {
            await _telegramMethods.SendMessageAsync(userId, $"{Emoji.DENIED}  You do not have messages left in your {labelId}.");
        }

        public async Task EmptyAllMessage(string userId)
        {
            await _telegramMethods.SendMessageAsync(userId, $"{Emoji.DENIED}  You do not have any messages left.");
        }

        public async Task GmailInlineInboxCommandMessage(string userId)
        {
            await _telegramMethods.SendMessageAsync(userId, $"@{_settings.BotName} Inbox:");
        }

        public async Task GmailInlineDraftCommandMessage(string userId)
        {
            await _telegramMethods.SendMessageAsync(userId, $"@{_settings.BotName} Draft:");
        }

        public async Task GmailInlineAllCommandMessage(string userId)
        {
            await _telegramMethods.SendMessageAsync(userId, $"@{_settings.BotName} Inbox:");
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
                    Id = message.IsDraft ? message.DraftId : message.Id,
                    Title = ShortMessageTitleFormatter(message.From.Name, message.From.Email, date),
                    Description = message.Subject,
                    InputMessageContent = new InputTextMessageContent
                    {
                        MessageText = message.IsDraft ? "Draft:" : "Message:"
                    }
                });
            }
            if (!offset.HasValue)
                await _telegramMethods.AnswerInlineQueryAsync(inlineQueryId, inlineQueryResults, 0, true);
            else
                await _telegramMethods.AnswerInlineQueryAsync(inlineQueryId, inlineQueryResults, 0, true, offset.ToString());
        }

        public async Task ShowShortEmptyAnswerInlineQuery(string inlineQueryId)
        {
            var inlineQueryResults = new List<InlineQueryResult>
            {
                new InlineQueryResultArticle
                {
                    Id = CallbackCommand.IGNORE_COMMAND,
                    Title = "No results matched your search.",
                    Description = "Try using search options such as sender, date, size and more.",
                    InputMessageContent = new InputTextMessageContent
                    {
                        MessageText = "There is no point in choosing this result"
                    }
                }
            };
            await _telegramMethods.AnswerInlineQueryAsync(inlineQueryId, inlineQueryResults, 0, true);
        }

        public async Task EditProceedMessage(string chatId, string messageId)
        {
            await _telegramMethods.EditMessageTextAsync("Success", chatId, messageId);
        }

        public async Task ShowShortMessageAsync(string chatId, FormattedMessage formattedMessage, string access = UserAccess.FULL)
        {
            formattedMessage.NullInspect(nameof(formattedMessage));

            var header = formattedMessage.Header;
            var message = Emoji.CLOSED_EMAIL_ENVELOP + header + $"{Environment.NewLine}{Environment.NewLine} {formattedMessage.Snippet}";
            var keyboard = access == UserAccess.FULL
                ? _getKeyboardFactory.CreateKeyboard(GetKeyboardState.Minimized, formattedMessage)
                : _getKeyboardFactory.CreateKeyboard(GetKeyboardState.Notify, formattedMessage);
            await _telegramMethods.SendMessageAsync(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public void ShowShortMessage(string chatId, FormattedMessage formattedMessage, string access = UserAccess.FULL)
        {
            formattedMessage.NullInspect(nameof(formattedMessage));

            var header = formattedMessage.Header;
            var message = Emoji.CLOSED_EMAIL_ENVELOP + header + $"{Environment.NewLine}{Environment.NewLine} {formattedMessage.Snippet}";
            var keyboard = access == UserAccess.FULL
                ? _getKeyboardFactory.CreateKeyboard(GetKeyboardState.Minimized, formattedMessage)
                : _getKeyboardFactory.CreateKeyboard(GetKeyboardState.Notify, formattedMessage);
            _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public async Task UpdateMessage(string chatId, int messageId, GetKeyboardState state, FormattedMessage formattedMessage, int page = 0, bool isIgnored = false)
        {
            formattedMessage.NullInspect(nameof(formattedMessage));

            var header = formattedMessage.Header;
            var keyboard = _getKeyboardFactory.CreateKeyboard(state, formattedMessage, page, isIgnored);
            var displayedMessage = page == 0
                ? Emoji.CLOSED_EMAIL_ENVELOP + header + $"{Environment.NewLine}{Environment.NewLine}{formattedMessage.Snippet}"
                : Emoji.RED_ARROWED_ENVELOPE + header + $"{Environment.NewLine}{Environment.NewLine}{formattedMessage.DesirableBody[page - 1]}";
            await _telegramMethods.EditMessageTextAsync(displayedMessage, chatId, messageId.ToString(), null, ParseMode.Html, null, keyboard);
        }

        public async Task SendAttachmentsListMessage(string chatId, int messageId, FormattedMessage formattedMessage, GetKeyboardState state, int page = 0)
        {
            formattedMessage.NullInspect(nameof(formattedMessage));
            if (!formattedMessage.HasAttachments)
                throw new InvalidOperationException($"{nameof(formattedMessage.HasAttachments)} property must equals true to avoid this exception.");

            var keyboard = _getKeyboardFactory.CreateKeyboard(state, formattedMessage, page);
            var messageTextBuilder = new StringBuilder($"Files attached to this message:{Environment.NewLine}");
            formattedMessage.Attachments.IndexEach((a, i) =>
            {
                messageTextBuilder.AppendLine($"{i + 1}. {a.FileName}");
            });
            messageTextBuilder.AppendLine();
            messageTextBuilder.Append("Please select a file by number to download it:");
            await _telegramMethods.EditMessageTextAsync(messageTextBuilder.ToString(), chatId, messageId.ToString(), null, null, null, keyboard);
        }

        public async Task ShowContactsAnswerInlineQuery(string inlineQueryId, IEnumerable<UserInfo> contacts, int? offset = null)
        {
            var inlineQueryResults = new List<InlineQueryResult>();
            foreach (var contact in contacts)
            {
                inlineQueryResults.Add(new InlineQueryResultArticle
                {
                    Id = $"{contact.Email}",
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
                await _telegramMethods.AnswerInlineQueryAsync(inlineQueryId, inlineQueryResults, 0, true);
            else
                await _telegramMethods.AnswerInlineQueryAsync(inlineQueryId, inlineQueryResults, 0, true, offset.ToString());
        }

        public async Task<TextMessage> SpecifyNewMailMessage(string chatId, SendKeyboardState state, NmStoreModel model = null)
        {
            var keyboard = _sendKeyboardFactory.CreateKeyboard(state, model);
            var message = BuildNewMailMessage(model, state);
            return await _telegramMethods.SendMessageAsync(chatId, message, ParseMode.Html, true, false, null, keyboard);
        }

        public async Task SaveAsDraftQuestionMessage(string chatId, SendKeyboardState state)
        {
            var keyboard = _sendKeyboardFactory.CreateKeyboard(state);
            await _telegramMethods.SendMessageAsync(chatId, _storeDraftMessageText, ParseMode.Html, false, false, null, keyboard);
        }

        public async Task UpdateNewMailMessage(string chatId, SendKeyboardState state, NmStoreModel model, string draftId = "", string errorMessage = "Unidentified error")
        {
            var keyboard = _sendKeyboardFactory.CreateKeyboard(state, model, draftId);
            string message = null;
            switch (state)
            {
                case SendKeyboardState.Drafted:
                    message = _restoreFromDraftMessageText;
                    break;
                case SendKeyboardState.SentSuccessful:
                    message = BuildNewMailMessage(model, state);
                    break;
                case SendKeyboardState.SentWithError:
                    message = $"<b>Error while sending a message:</b>{Environment.NewLine}{errorMessage}";
                    break;
                default:
                    message = BuildNewMailMessage(model, state);
                    break;
            }

            await _telegramMethods.EditMessageTextAsync(message, chatId, model.MessageId.ToString(), null, ParseMode.Html, true, keyboard);
        }

        public async Task SendAttachmentToChat(string chatId, string fullFileName, string caption)
        {
            await _telegramMethods.SendDocument(chatId, fullFileName, caption);
        }

        public async Task SendLostInfoMessage(string chatId)
        {
            var message = $"{Emoji.WHITE_EXCLAMATION} Info about this message is lost.";
            await _telegramMethods.SendMessageAsync(chatId, message);
        }

        public async Task NotRecognizedEmailMessage(string chatId, string email)
        {
            var message = $"{Emoji.WHITE_EXCLAMATION} The address {email} was not recognized.";
            await _telegramMethods.SendMessageAsync(chatId, message);
        }

        public async Task ChangeTextMessageForceReply(string chatId)
        {
            var reply = new ForceReply
            {
                Selective = false
            };
            var message = $"<b>{ForceReplyCommand.MESSAGE_COMMAND} </b>\r\n{Emoji.INFO_SIGN}<i>To attach files drop them into the chat.</i>";

            await _telegramMethods.SendMessageAsync(chatId, message, ParseMode.Html, false, false, null, reply);
        }

        public async Task ChangeSubjectForceReply(string chatId)
        {
            var reply = new ForceReply
            {
                Selective = false
            };
            var message = $"<b>{ForceReplyCommand.SUBJECT_COMMAND} </b>";

            await _telegramMethods.SendMessageAsync(chatId, message, ParseMode.Html, false, false, null, reply);
        }

        public async Task DownloadFile(File file, string localPath)
        {
            await _telegramMethods.DownloadFileAsync(file, localPath);
        }


        public async Task<File> GetFile(string fileId)
        {
            return await _telegramMethods.GetFileAsync(fileId);
        }

        public async Task SendErrorAboutMaxAttachmentSizeToChat(string chatId, string fileName)
        {
            throw new NotImplementedException("error");
        }

        public async Task DraftSavedMessage(string chatId, bool notSaved = false)
        {
            var not = notSaved ? "not" : "";
            var message = $"{Emoji.OK} Draft {not} saved!";
            await _telegramMethods.SendMessageAsync(chatId, message);
        }

        public async Task RemoveKeyboard(string chatId)
        {
            var removeKeyboard = new ReplyKeyboardRemove();
            await _telegramMethods.SendMessageAsync(chatId, "/delete", null, false, false, null, removeKeyboard);
        }

        public async Task DeleteMessage(string chatId, int messageId)
        {

        }

        public async Task ShowSettingsMenu(string chatId)
        {
            var keyboard = _settingsKeyboardFactory.CreateKeyboard(SettingsKeyboardState.MainMenu);
            var message = SettingsMenuMessageBuilder(SettingsKeyboardState.MainMenu);
            await _telegramMethods.SendMessageAsync(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public async Task UpdateSettingsMenu(string chatId, int messageId, SettingsKeyboardState state, SelectedOption option = default(SelectedOption), UserSettingsModel userSettings = null)
        {
            var keyboard = _settingsKeyboardFactory.CreateKeyboard(state);
            var message = SettingsMenuMessageBuilder(state, option, userSettings);
            await
                _telegramMethods.EditMessageTextAsync(message, chatId, messageId.ToString(), null, ParseMode.Html, null, keyboard);
        }

        public async Task CreateNewLabelForceReply(string chatId)
        {
            var reply = new ForceReply();
            var message = $"<b>{ForceReplyCommand.NEW_LABEL_COMMAND}</b>";
            await _telegramMethods.SendMessageAsync(chatId, message, ParseMode.Html, false, false, null, reply);
        }

        public async Task CreateLabelSuccessful(string chatId, string labelName)
        {
            await _telegramMethods.SendMessageAsync(chatId, $"Label {labelName} created successfully.");
        }

        public async Task CreateLabelError(string chatId, string labelName)
        {
            await _telegramMethods.SendMessageAsync(chatId, $"Label {labelName} was not created because of an error.");
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

        private string BuildNewMailMessage(NmStoreModel model, SendKeyboardState state)
        {
            string headerText;
            switch (state)
            {
                case SendKeyboardState.Init:
                case SendKeyboardState.Continue:
                    headerText = _newMessageMainText;
                    break;
                case SendKeyboardState.SentSuccessful:
                    headerText = _newMessageSuccessfulSentText;
                    break;
                default:
                    headerText = "";
                    break;
            }
            var message = new StringBuilder(headerText);
            if (model == null)
            {
                message.AppendLine(_newMessageTipText);
                return message.ToString();
            }
            var iterFunc = new Action<StringBuilder, List<string>, string>((builder, collection, label) =>
            {
                if (collection == null || !collection.Any()) return;
                builder.Append($"<b>{label}:</b> ");
                collection.IndexEach((item, i) =>
                {
                    builder.Append($"<code>{Path.GetFileName(item)}</code>"); //! GetFileName 
                    if (i < collection.Count - 1) builder.Append(", ");
                });
            });
            message.AppendLine();
            iterFunc(message, model.To.Select(a => a.Email).ToList(), "To");
            message.AppendLine();
            iterFunc(message, model.Cc.Select(a => a.Email).ToList(), "Cc");
            message.AppendLine();
            iterFunc(message, model.Bcc.Select(a => a.Email).ToList(), "Bcc");
            message.AppendLine();
            if (model.Subject != null)
                message.AppendLine($"<b>Subject:</b> {model.Subject}");
            if (model.Message != null)
            {
                message.AppendLine("<b>Message:</b>");
                message.AppendLine(model.Message);
            }
            message.AppendLine();
            iterFunc(message, model.File.Select(f => f.OriginalName).ToList(), $"{Emoji.PAPER_CLIP}Attachments"); //Emoji probable cause of error, because it will be send inside <b> tag
            return message.ToString();
        }

        private string SettingsMenuMessageBuilder(SettingsKeyboardState state, SelectedOption option = default(SelectedOption), UserSettingsModel userSettings = null)
        {
            if (userSettings == null && state.EqualsAny(
                SettingsKeyboardState.Labels,
                SettingsKeyboardState.Ignore,
                SettingsKeyboardState.WhiteList,
                SettingsKeyboardState.BlackList,
                SettingsKeyboardState.EditLabelsList))
                throw new InvalidOperationException($"{nameof(userSettings)} must be not null if {nameof(state)} equals {state}.");

            StringBuilder message = new StringBuilder();
            switch (state)
            {
                case SettingsKeyboardState.MainMenu:
                    switch (option)
                    {
                        case SelectedOption.Option9: //about button
                            message.AppendLine($"<b>{_settings.BotName}</b>");
                            message.AppendLine();
                            message.AppendLine();
                            message.AppendLine($"Bot verison: {_settings.BotVersion}");
                            message.AppendLine("Developed by Igor 'CoffeeJelly' Salzhenitsin");
                            message.AppendLine("Contact emails:");
                            message.AppendLine("<code>dnm.nova@gmail.com</code>");
                            message.AppendLine("<code>igor.sal@protonmail.com</code>");
                            message.AppendLine();
                            message.AppendLine();
                            message.AppendLine("2017");
                            break;
                        default:
                            message.Append("<b>Main Settings Menu</b>");
                            break;
                    }
                    break;
                case SettingsKeyboardState.Labels:
                    switch (option)
                    {
                        default:
                            message.AppendLine("<b>Labels Menu</b>");
                            message.AppendLine();
                            message.Append(!userSettings.UseWhitelist
                                ? "Choose <b>Whitelist</b> to specify the email labels that will be allowed. "
                                + "Incoming email without at least one of the selected labels will not be displayed in the Telegram chat."
                                : "Choose <b>Blacklist</b> to specify the email labels that will be blocked."
                                + "Incoming email with at least one of the selected labels will not be displayed in the Telegram chat.");
                            break;
                    }
                    break;
                case SettingsKeyboardState.EditLabelsList:
                    break;
                case SettingsKeyboardState.WhiteList:
                        message.AppendLine("<b>Whitelist</b>");
                        message.AppendLine();
                    message.AppendLine(!userSettings.UseWhitelist
                        ? "If you want to use whitelist click \"Use whitelist mode\" button."
                        : "Click the button to add it to (or remove from) the whitelist.");
                    break;
                case SettingsKeyboardState.BlackList:
                    message.AppendLine("<b>Blacklist</b>");
                    message.AppendLine();
                    message.AppendLine(userSettings.UseWhitelist
                        ? "If you want to use blacklist click \"Use blacklist mode\" button."
                        : "Click the button to add it to (or remove from) the whitelist.");
                    break;
                case SettingsKeyboardState.Ignore:
                    switch (option)
                    {
                        case SelectedOption.Option1:
                            message.AppendLine(userSettings.IgnoreList?.Count == 0
                                ? "Your ignore list is empty."
                                : $"You have {userSettings.IgnoreList?.Count} email(s) ignored:");
                            userSettings.IgnoreList?.ForEach(email =>
                                {
                                    message.AppendLine(email.Address);
                                });
                            break;
                        default:
                            message.AppendLine("<b>Ignore Control Menu</b>");
                            message.AppendLine();
                            break;
                    }
                    break;
                case SettingsKeyboardState.Permissions:
                    message.AppendLine("<b>Permissions Menu</b>");
                    message.AppendLine();
                    message.AppendLine("You can change or revoke the bot permissions to your Gmail account here.");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            return message.ToString();
        }

        private readonly TelegramMethods _telegramMethods;
        private readonly string _newMessageMainText =
                        $"{Emoji.NEW} Please specify the <b>Recipients</b>, a <b>Subject</b> and the <b>Content</b> of the email: ";

        private readonly string _newMessageSuccessfulSentText = $"{Emoji.OK} Message sent successfully!";

        private readonly string _newMessageTipText = $"{Emoji.INFO_SIGN} You can use quick command, just type in the chat:" +
                        $"{Environment.NewLine}<i>/new \"recipient1@gmail.com, recipient2@gmail.com,...\" \"subject\" \"email text\"</i>" +
                        $"{Environment.NewLine}and press Enter to quick send the email." +
                        $"{Environment.NewLine}{Emoji.INFO_SIGN} For multiple recipients use comma separator.";

        private readonly string _storeDraftMessageText =
            $"{Emoji.QUESTION_SIGN} You have already started to create a new message. " +
            $"You can save it as draft and create new instance of new message or continue composing.";

        private readonly string _restoreFromDraftMessageText =
            $"{Emoji.INFO_SIGN} To restore message from draft and continue composing click this button {Emoji.DOWN_ARROW}";

        private readonly BotSettings _settings = BotInitializer.Instance.BotSettings;
        private readonly string _contactsThumbUrl;
        private readonly GetKeyboardFactory _getKeyboardFactory = new GetKeyboardFactory();
        private readonly SendKeyboardFactory _sendKeyboardFactory = new SendKeyboardFactory();
        private readonly SettingsKeyboardFactory _settingsKeyboardFactory = new SettingsKeyboardFactory();
    }
}