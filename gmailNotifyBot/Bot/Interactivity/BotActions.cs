﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.General;
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
using Fody;
using File = CoffeeJelly.TelegramBotApiWrapper.Types.General.File;
using TelegramMethods = CoffeeJelly.TelegramBotApiWrapper.Methods.TelegramMethods;
using GmailLabel = Google.Apis.Gmail.v1.Data.Label;


namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity
{
    [ConfigureAwait(false)]
    internal class BotActions
    {
        public BotActions(string token)
        {
            token.NullInspect(nameof(token));
            _telegramMethods = new TelegramMethods(token);
#if DEBUG
            _contactsThumbUrl = @"https://i.imgur.com/0shhQ5U.jpg";
            _closedEnvelopeThumbUrl = @"https://i.imgur.com/ljODIM2.jpg";
            _openEnvelopeThumbUrl = @"https://i.imgur.com/OFYvQO4.jpg";
            //_closedEnvelopeThumbUrl = @"https://static.wixstatic.com/media/60593c_b482c73f21cc4b8fa6616ee46a908f0b~mv2.jpg";
            //_openEnvelopeThumbUrl =
            //@"https://image.freepik.com/free-icon/open-e-mail-message-envelope-symbol-of-ios-7-interface_318-35260.jpg";
#else
            _contactsThumbUrl = $@"https://{_settings.DomainName}/{_settings.ImagesPath}Silhouette49.jpg";
            _closedEnvelopeThumbUrl = $@"https://{_settings.DomainName}/{_settings.ImagesPath}ClosedEnvelope3.jpg";
            _openEnvelopeThumbUrl = $@"https://{_settings.DomainName}/{_settings.ImagesPath}OpenedEnvelope3.jpg";
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
            await _telegramMethods.SendMessage(userId, @"I am lost your credentials. Please reauthorize me using /connect command or click a button below.", null, false, false, null, keyboard);
        }

        public async Task AuthorizeMessage(string userId, Uri notifyAccessUri = null, Uri fullAccessUri = null)
        {
            var notifyAccessButton = new InlineKeyboardButton
            {
                Text = "Mail Notify",
                Url = notifyAccessUri?.OriginalString
            };
            var fullAccessButton = new InlineKeyboardButton
            {
                Text = "Full Access",
                Url = fullAccessUri?.OriginalString
            };
            var row = new List<InlineKeyboardButton>();
            if (notifyAccessUri != null)
                row.Add(notifyAccessButton);
            if (fullAccessUri != null)
                row.Add(fullAccessButton);

            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>> { row }
            };
            await _telegramMethods.SendMessage(userId,
                $"Open one of this link to authorize the bot to get: ", null, false, false, null, keyboard);
        }

        public async Task AuthorizationTimeExpiredMessage(string userId)
        {
            await
                _telegramMethods.SendMessage(userId,
                    @"Time for authorization has expired. Please type again /connect command.");
        }

        public async Task AuthorizationFailedMessage(string userId)
        {
            await
                _telegramMethods.SendMessage(userId, "Authorization failed. See ya!");
        }

        public async Task AuthorizationSuccessfulMessage(string userId)
        {
            await
                _telegramMethods.SendMessage(userId, "Authorization successful! Now you can receive notifications about new emails and use other functions!");
        }

        public async Task AuthorizationErrorMessage(string userId)
        {
            await
                _telegramMethods.SendMessage(userId, "I can't send you the authorization link, I'm so sorry!");
        }

        public async Task EmailAddressMessage(string userId, string emailAddress)
        {
            await _telegramMethods.SendMessage(userId, emailAddress);
        }

        public async Task EmptyLabelMessage(string userId, string labelId)
        {
            await _telegramMethods.SendMessage(userId, $"{Emoji.DENIED}  You do not have messages left in your {labelId}.");
        }

        public async Task EmptyAllMessage(string userId)
        {
            await _telegramMethods.SendMessage(userId, $"{Emoji.DENIED}  You do not have any messages left.");
        }

        public async Task StartMessage(string userId)
        {
            var message = new StringBuilder();
            message.AppendLine("Hi! I am a LazyMailBot. I can notify you about incoming emails in your Gmail. " +
                          "In \"full mode\" i also can be something like an email client and manage your mailbox right from the chat.");
            message.AppendLine();
            message.AppendLine(@"Start with /connect command to Authorize bot with your Gmail account via OAuth 2.0");
            message.AppendLine(@"To get full list of available commands type /help");
            await _telegramMethods.SendMessage(userId, message.ToString());
        }

        public async Task StartMessage(string userId, string emailAddress)
        {
            var message = $"You are already autorized as {emailAddress}";
            var button = new InlineKeyboardButton
            {
                Text = "Authorize another account",
                CallbackData = new GetCallbackData
                {
                    Command = TextCommand.AUTHORIZE_COMMAND
                }
            };
            var keyboard = new InlineKeyboardMarkup
            {
                InlineKeyboard = new List<List<InlineKeyboardButton>> { new List<InlineKeyboardButton> { button } }
            };
            await _telegramMethods.SendMessage(userId, message, null, false, false, null, keyboard);
        }

        public async Task HelpMessage(string userId)
        {
            var message = new StringBuilder();
            message.AppendLine("Available commands:");
            message.AppendLine(@"/connect - Authorize bot with Gmail via OAuth 2.0");
            message.AppendLine(@"/new - Compose new email (active in full mode)");
            message.AppendLine(@"/stop - Stop notifications");
            message.AppendLine(@"/resume - Resume notifications");
            message.AppendLine(@"/settings - Open settings menu");
            message.AppendLine(@"/help - show help");
            message.AppendLine();
            message.AppendLine("You can also use inline queries in full mode, just type the name of the bot and one " +
                               "of the commands below, as in the example:");
            message.AppendLine("<i>@lazymailbot inbox</i> - This query displays a list of your inbox emails");
            message.AppendLine("After command u can specify a search expression or use a special argument \"s:\" " +
                               "to skip the number of results in the issuance:");
            message.AppendLine("<i>@lazymailbot all s:100</i> - This query displays a list of all emails, skipping the first 100.");
            message.AppendLine("Available inline commands:");
            message.AppendLine("all - Get all emails list");
            message.AppendLine("inbox - Get inbox emails list");
            message.AppendLine("draft - Get list of drafts");
            await _telegramMethods.SendMessage(userId, message.ToString(), ParseMode.Html);
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
                    Id = message.IsDraft ? message.DraftId : message.MessageId,
                    Title = ShortMessageTitleFormatter(message.From.Name, message.From.Email, date),
                    Description = message.Subject,
                    InputMessageContent = new InputTextMessageContent
                    {
                        MessageText = message.IsDraft ? "Draft:" : "Message:"
                    },
                    ThumbUrl = message.LabelIds.Any(l => l == Label.Unread)
                                ? _closedEnvelopeThumbUrl
                                : _openEnvelopeThumbUrl
                });
            }
            if (!offset.HasValue)
                await _telegramMethods.AnswerInlineQuery(inlineQueryId, inlineQueryResults, 0, true);
            else
                await _telegramMethods.AnswerInlineQuery(inlineQueryId, inlineQueryResults, 0, true, offset.ToString());
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
            await _telegramMethods.AnswerInlineQuery(inlineQueryId, inlineQueryResults, 0, true);
        }

        public async Task EditProceedMessage(string chatId, string messageId)
        {
            await _telegramMethods.EditMessageText("Success", chatId, messageId);
        }

        public async Task ShowShortMessageAsync(string chatId, FormattedMessage formattedMessage, bool fullAccess = true)
        {
            formattedMessage.NullInspect(nameof(formattedMessage));

            var header = formattedMessage.Header;
            var message = Emoji.CLOSED_EMAIL_ENVELOP + header + $"{Environment.NewLine}{Environment.NewLine} {formattedMessage.Snippet}";
            var keyboard = fullAccess
                ? _getKeyboardFactory.CreateKeyboard(GetKeyboardState.Minimized, formattedMessage)
                : _getKeyboardFactory.CreateKeyboard(GetKeyboardState.Notify, formattedMessage);
            await _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public void ShowShortMessage(string chatId, FormattedMessage formattedMessage, string access = UserAccess.FULL)
        {
            formattedMessage.NullInspect(nameof(formattedMessage));

            var header = formattedMessage.Header;
            var message = Emoji.CLOSED_EMAIL_ENVELOP + header + $"{Environment.NewLine}{Environment.NewLine} {formattedMessage.Snippet}";
            var keyboard = access == UserAccess.FULL
                ? _getKeyboardFactory.CreateKeyboard(GetKeyboardState.Minimized, formattedMessage)
                : _getKeyboardFactory.CreateKeyboard(GetKeyboardState.Notify, formattedMessage);
            var temp = _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, keyboard).Result;
        }

        public async Task UpdateMessage(string chatId, int messageId, GetKeyboardState state, FormattedMessage formattedMessage, int page = 0, bool isIgnored = false)
        {
            formattedMessage.NullInspect(nameof(formattedMessage));

            var header = formattedMessage.Header;
            var keyboard = _getKeyboardFactory.CreateKeyboard(state, formattedMessage, page, isIgnored);
            var displayedMessage = page == 0
                ? Emoji.CLOSED_EMAIL_ENVELOP + header + $"{Environment.NewLine}{Environment.NewLine}{formattedMessage.Snippet}"
                : Emoji.RED_ARROWED_ENVELOPE + header + $"{Environment.NewLine}{Environment.NewLine}{formattedMessage.DesirableBody[page - 1]}";
            await _telegramMethods.EditMessageText(displayedMessage, chatId, messageId.ToString(), null, ParseMode.Html, null, keyboard);
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
            await _telegramMethods.EditMessageText(messageTextBuilder.ToString(), chatId, messageId.ToString(), null, null, null, keyboard);
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
                await _telegramMethods.AnswerInlineQuery(inlineQueryId, inlineQueryResults, 0, true);
            else
                await _telegramMethods.AnswerInlineQuery(inlineQueryId, inlineQueryResults, 0, true, offset.ToString());
        }

        public async Task<TextMessage> SpecifyNewMailMessage(string chatId, SendKeyboardState state, NmStoreModel model = null)
        {
            var keyboard = _sendKeyboardFactory.CreateKeyboard(state, model);
            var message = BuildNewMailMessage(model, state);
            return await _telegramMethods.SendMessage(chatId, message, ParseMode.Html, true, false, null, keyboard);
        }

        public async Task SaveAsDraftQuestionMessage(string chatId, SendKeyboardState state)
        {
            var keyboard = _sendKeyboardFactory.CreateKeyboard(state);
            await _telegramMethods.SendMessage(chatId, _storeDraftMessageText, ParseMode.Html, false, false, null, keyboard);
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

            await _telegramMethods.EditMessageText(message, chatId, model.MessageId.ToString(), null, ParseMode.Html, true, keyboard);
        }

        public async Task SendAttachmentToChat(string chatId, string fullFileName, string caption)
        {
            await _telegramMethods.SendDocument(chatId, fullFileName, caption);
        }

        public async Task SendLostInfoMessage(string chatId)
        {
            var message = $"{Emoji.WHITE_EXCLAMATION} Info about this message is lost.";
            await _telegramMethods.SendMessage(chatId, message);
        }

        public async Task NotRecognizedEmailMessage(string chatId, string email)
        {
            var message = $"{Emoji.WHITE_EXCLAMATION} The address {email} was not recognized.";
            await _telegramMethods.SendMessage(chatId, message);
        }

        public async Task EmailAbsentInIgnoreMessage(string chatId, string email)
        {
            var message = $"{Emoji.WHITE_EXCLAMATION} The email address {email} absent in the ignore list.";
            await _telegramMethods.SendMessage(chatId, message);
        }

        public async Task EmailAbsentInIgnoreMessage(string chatId, int number)
        {
            var message = $"{Emoji.WHITE_EXCLAMATION} Ignore list does not contain an email address with a sequence number equal to {number}.";
            await _telegramMethods.SendMessage(chatId, message);
        }

        public async Task ChangeTextMessageForceReply(string chatId)
        {
            var reply = new ForceReply
            {
                Selective = false
            };
            var message = $"<b>{ForceReplyCommand.MESSAGE_COMMAND} </b>\r\n{Emoji.INFO_SIGN}<i>To attach files drop them into the chat.</i>";

            await _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, reply);
        }

        public async Task ChangeSubjectForceReply(string chatId)
        {
            var reply = new ForceReply
            {
                Selective = false
            };
            var message = $"<b>{ForceReplyCommand.SUBJECT_COMMAND} </b>";

            await _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, reply);
        }

        public async Task DownloadFile(File file, string localPath)
        {
            await _telegramMethods.DownloadFileAsync(file, localPath);
        }


        public async Task<File> GetFile(string fileId)
        {
            return await _telegramMethods.GetFile(fileId);
        }

        public async Task SendErrorAboutMaxAttachmentSizeToChat(string chatId, string fileName)
        {
            var message = $"{Emoji.CROSS_MARK} {fileName} is too big. The maximum attachment size is {_settings.MaxAttachmentSize}";
            await _telegramMethods.SendMessage(chatId, message);
        }

        public async Task DraftSavedMessage(string chatId, bool notSaved = false)
        {
            var not = notSaved ? "not" : "";
            var message = $"{Emoji.OK} Draft {not} saved!";
            await _telegramMethods.SendMessage(chatId, message);
        }

        public async Task RemoveKeyboard(string chatId)
        {
            var removeKeyboard = new ReplyKeyboardRemove();
            var message = await _telegramMethods.SendMessage(chatId, TextCommand.DELETE_MSG_MARK, null, false, false, null, removeKeyboard);
            await _telegramMethods.DeleteMessage(chatId, message.MessageId);
        }

        public async Task DeleteMessage(string chatId, int messageId)
        {
            await _telegramMethods.DeleteMessage(chatId, messageId);
        }

        public async Task ShowSettingsMenu(string chatId, UserSettingsModel settings)
        {
            var keyboard = _settingsKeyboardFactory.CreateKeyboard(SettingsKeyboardState.MainMenu, settings);
            var message = SettingsMenuMessageBuilder(SettingsKeyboardState.MainMenu);
            await _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public async Task ShowSettingsMenu(string chatId, SettingsKeyboardState state,
             SelectedOption option = default(SelectedOption), UserSettingsModel model = null, IEnumerable<LabelInfo> labels = null)
        {
            var keyboard = _settingsKeyboardFactory.CreateKeyboard(state, model, labels);
            var message = SettingsMenuMessageBuilder(state, option, model);
            await _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public async Task UpdateSettingsMenu(string chatId, int messageId, SettingsKeyboardState state, SelectedOption option = default(SelectedOption),
              UserSettingsModel model = null, TempDataModel tempData = null, IEnumerable<LabelInfo> labels = null)
        {
            var keyboard = _settingsKeyboardFactory.CreateKeyboard(state, model, labels);
            var message = SettingsMenuMessageBuilder(state, option, model, tempData?.LabelId);
            await
                _telegramMethods.EditMessageText(message, chatId, messageId.ToString(), null, ParseMode.Html, null, keyboard);
        }

        public async Task CreateNewLabelForceReply(string chatId)
        {
            var reply = new ForceReply();
            var message = $"<b>{ForceReplyCommand.NEW_LABEL_COMMAND}</b>";
            await _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, reply);
        }

        public async Task EditLabelNameForceReply(string chatId)
        {
            var reply = new ForceReply();
            var message = $"<b>{ForceReplyCommand.EDIT_LABEL_NAME_COMMAND}</b>";
            await _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, reply);
        }

        public async Task AddToIgnoreForceReply(string chatId)
        {
            var reply = new ForceReply();
            var message = $"<b>{ForceReplyCommand.ADD_TO_IGNORE_COMMAND}</b>" +
                "Type email address here.";
            await _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, reply);
        }

        public async Task RemoveFromIgnoreForceReply(string chatId)
        {
            var reply = new ForceReply();
            var message = $"<b>{ForceReplyCommand.REMOVE_FROM_IGNORE_COMMAND}</b>" +
                $"\r\n{Emoji.INFO_SIGN}<i>You can enter here an email address or a sequence number in the ignore list." +
                $"\r\nTo see all emails and their number - choose {IgnoreMenuButtonCaption.Show} option in the Ignore menu.</i>";
            await _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, reply);
        }

        public async Task CreateLabelSuccessful(string chatId, string labelName)
        {
            await _telegramMethods.SendMessage(chatId, $"Label {labelName} created successfully.");
        }

        public async Task AddToIgnoreListSuccessMessage(string chatId, string emailAddress)
        {
            await _telegramMethods.SendMessage(chatId, $"{emailAddress} added to the ignore list.");
        }

        public async Task AlreadyInIgnoreListMessage(string chatId, string emailAddress)
        {
            await _telegramMethods.SendMessage(chatId, $"{emailAddress} already exists in the ignore list.");
        }

        public async Task RemoveFromIgnoreListSuccessMessage(string chatId, string emailAddress)
        {
            await _telegramMethods.SendMessage(chatId, $"{emailAddress} removed from the ignore list.");
        }

        public async Task CreateLabelError(string chatId, string labelName)
        {
            await _telegramMethods.SendMessage(chatId, $"Label {labelName} was not created because of an error.");
        }

        public async Task RevokeTokenSuccessfulMessage(string chatId)
        {
            await _telegramMethods.SendMessage(chatId, $"The {_settings.BotName}'s permissions to your account has been revorked." +
                $"\r\nTo see apps connected to your account visit {AccountPermissionsUrl}" +
                $" \r\nThanks for using this bot, see you!");
        }

        public async Task RevokeTokenUnSuccessfulMessage(string chatId)
        {
            await _telegramMethods.SendMessage(chatId, $"Can't revoke permissions, please try to revoke permissions via web browser.");
        }

        public async Task SendAccountPermissinsUrl(string chatId)
        {
            var message =
                $"To revoke permissions via web browser <a href=\"{AccountPermissionsUrl}\">open this link</a>";
            await _telegramMethods.SendMessage(chatId, message, ParseMode.Html);
        }

        public async Task NotificationStartedMessage(string chatId)
        {
            var message = $"{Emoji.GRAY_CHECKED_BOX} Notifications about new emails is active now!";
            await _telegramMethods.SendMessage(chatId, message, ParseMode.Html);
        }

        public async Task NotificationStopedMessage(string chatId)
        {
            var message = "You will no longer receive notifications about new emails.";
            var keyboard = _generalKeyboardFactory.CreateKeyboard(GeneralKeyboardState.ResumeNotifications);
            await _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public async Task SendSaveMessageAsDraftError(string chatId)
        {
            var message = $"{Emoji.CROSS_MARK} Error to save message as draft.";
            await _telegramMethods.SendMessage(chatId, message);
        }

        public async Task<bool> StopProgressBar(string chatId)
        {
            return await _telegramMethods.AnswerCallbackQuery(chatId);
        }

        public async Task<bool> ErrorOperation(string chatId)
        {
            var message = $"{Emoji.FROWNING_FACE} Error..";
            return await _telegramMethods.AnswerCallbackQuery(chatId, message);
        }

        public async Task NewMessageSentSuccessfull(string chatId)
        {
            var message = "Message sent successfully!";
            await _telegramMethods.SendMessage(chatId, message);
        }

        public async Task NewMessageArgumentsError(string chatId)
        {
            var message = new StringBuilder();
            message.AppendLine("Your query is wrong!");
            message.AppendLine("Please specify all arguments as in the example below(recipients, subject and text):");
            message.AppendLine();
            message.AppendLine(
                "<i>/new \"recipient1@gmail.com, recipient2@gmail.com,...\" \"subject\" \"email text\"</i>");
            message.AppendLine();
            message.AppendLine("Also arguments must be in double quotes");
            await _telegramMethods.SendMessage(chatId, message.ToString(), ParseMode.Html);
        }

        public async Task NewMessageRecipientsArgumentError(string chatId)
        {
            var message = new StringBuilder();
            message.AppendLine("Your recipients argument is wrong!");
            message.AppendLine("Recipients must be a comma-separated enumeration and must be a valid email addresses.");
            await _telegramMethods.SendMessage(chatId, message.ToString());
        }

        public async Task NewMessageSubjectArgumentError(string chatId)
        {
            var message = new StringBuilder();
            message.AppendLine("Your subject argument is wrong!");
            message.AppendLine("Subject must be not empty and contain something other than empty characters.");
            await _telegramMethods.SendMessage(chatId, message.ToString());
        }

        public async Task NewMessageTextArgumentError(string chatId)
        {
            var message = new StringBuilder();
            message.AppendLine("Your subject argument is wrong!");
            message.AppendLine("Text must be not empty and contain something other than empty characters.");
            await _telegramMethods.SendMessage(chatId, message.ToString());
        }

        private string ShortMessageTitleFormatter(string senderName, string senderEmail, string date)
        {
            const int maxLine = 44;

            var builder = new StringBuilder(maxLine);
            builder.Append(date);
            builder.Append(' ');
            builder.Append(senderName);
            if (maxLine - builder.Length > senderEmail.Length + 2)
                builder.Append($" {senderEmail}");
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

        private string SettingsMenuMessageBuilder(SettingsKeyboardState state, SelectedOption option = default(SelectedOption),
            UserSettingsModel userSettings = null, string editableLableId = null)
        {
            if (userSettings == null && state.EqualsAny(
                SettingsKeyboardState.LabelsMenu,
                SettingsKeyboardState.IgnoreMenu,
                SettingsKeyboardState.WhiteListMenu,
                SettingsKeyboardState.BlackListMenu))
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
                            message.AppendLine($"Bot version: {_settings.BotVersion}");
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
                case SettingsKeyboardState.LabelsMenu:
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
                case SettingsKeyboardState.EditLabelsMenu:
                    message.AppendLine("<b>User Defined Editable Labels:</b>");
                    message.AppendLine();
                    break;
                case SettingsKeyboardState.WhiteListMenu:
                    message.AppendLine("<b>Whitelist</b>");
                    message.AppendLine();
                    message.AppendLine(!userSettings.UseWhitelist
                        ? "If you want to use whitelist click \"Use whitelist mode\" button."
                        : "Click the button to add it to (or remove from) the whitelist.");
                    break;
                case SettingsKeyboardState.BlackListMenu:
                    message.AppendLine("<b>Blacklist</b>");
                    message.AppendLine();
                    message.AppendLine(userSettings.UseWhitelist
                        ? "If you want to use blacklist click \"Use blacklist mode\" button."
                        : "Click the button to add it to (or remove from) the blacklist.");
                    break;
                case SettingsKeyboardState.IgnoreMenu:
                    switch (option)
                    {
                        case SelectedOption.Option1:
                            message.AppendLine(userSettings.IgnoreList?.Count == 0
                                ? "Your ignore list is empty."
                                : $"You have {userSettings.IgnoreList?.Count} email(s) ignored:");
                            userSettings.IgnoreList?.IndexEach((email, i) =>
                                {
                                    message.AppendLine($"{i + 1}. {email.Address}");
                                });
                            break;
                        default:
                            message.AppendLine("<b>Ignore Control Menu</b>");
                            message.AppendLine();
                            message.AppendLine(
                                "To stop receiving notifications about new emails from a specific email address, " +
                                "add it to the ignore list.");
                            message.AppendLine("To add or remove an email from the ignore list, click the button and type the email address.");
                            message.AppendLine();
                            message.AppendLine($"{Emoji.INFO_SIGN} You can type the displayed sequence number to remove the email from the ignore list.");
                            break;
                    }
                    break;
                case SettingsKeyboardState.AdditionalMenu:
                    message.Append("<b>Main Settings Menu</b>");
                    break;
                case SettingsKeyboardState.PermissionsMenu:
                    message.AppendLine("<b>Permissions Menu</b>");
                    message.AppendLine();
                    message.AppendLine("You can change or revoke the bot permissions to your Gmail account here.");
                    break;
                case SettingsKeyboardState.LabelActionsMenu:
                    message.AppendLine($"<b>Edit label with id {editableLableId}:</b>");
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
                        $"{Environment.NewLine}" +
                        $"{Environment.NewLine}<i>/new \"recipient1@gmail.com, recipient2@gmail.com,...\" \"subject\" \"email text\"</i>" +
                        $"{Environment.NewLine}" +
                        $"{Environment.NewLine}and press Enter to quick send the email." +
                        $"{Environment.NewLine}{Emoji.INFO_SIGN} For multiple recipients use comma separator.";

        private readonly string _storeDraftMessageText =
            $"{Emoji.QUESTION_SIGN} You have already started to create a new message. " +
            $"You can save it as draft and create new instance of new message or continue composing.";

        private readonly string _restoreFromDraftMessageText =
            $"{Emoji.INFO_SIGN} To restore message from draft and continue composing click this button {Emoji.DOWN_ARROW}";

        private readonly string AccountPermissionsUrl = @"https://myaccount.google.com/permissions";

        private readonly BotSettings _settings = BotInitializer.Instance.BotSettings;
        private readonly string _contactsThumbUrl;
        private readonly string _closedEnvelopeThumbUrl;
        private readonly string _openEnvelopeThumbUrl;
        private readonly GetKeyboardFactory _getKeyboardFactory = new GetKeyboardFactory();
        private readonly SendKeyboardFactory _sendKeyboardFactory = new SendKeyboardFactory();
        private readonly SettingsKeyboardFactory _settingsKeyboardFactory = new SettingsKeyboardFactory();
        private readonly GeneralKeyboardFactory _generalKeyboardFactory = new GeneralKeyboardFactory();
    }
}