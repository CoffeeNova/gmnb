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
                    Command = Commands.AUTHORIZE_COMMAND
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
            await _telegramMethods.SendMessageAsync(userId, $"{Emoji.Denied}  You do not have messages left in your {labelId}.");
        }

        public async Task EmptyAllMessage(string userId)
        {
            await _telegramMethods.SendMessageAsync(userId, $"{Emoji.Denied}  You do not have any messages left.");
        }

        public async Task GmailInlineInboxCommandMessage(string userId)
        {
            await _telegramMethods.SendMessageAsync(userId, $"@{_settings.BotName} Inbox:");
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

        public async Task EditProceedMessage(string chatId, string messageId)
        {
            await _telegramMethods.EditMessageTextAsync("Success", chatId, messageId);
        }

        public async Task ShowShortMessageAsync(string chatId, FormattedMessage formattedMessage)
        {
            formattedMessage.NullInspect(nameof(formattedMessage));

            var header = formattedMessage.Header;
            var message = Emoji.ClosedEmailEnvelop + header + $"{Environment.NewLine}{Environment.NewLine} {formattedMessage.Snippet}";
            var keyboard = _getKeyboardFactory.CreateKeyboard(GetKeyboardState.Minimized, formattedMessage);
            await _telegramMethods.SendMessageAsync(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public void ShowShortMessage(string chatId, FormattedMessage formattedMessage)
        {
            formattedMessage.NullInspect(nameof(formattedMessage));

            var header = formattedMessage.Header;
            var message = Emoji.ClosedEmailEnvelop + header + $"{Environment.NewLine}{Environment.NewLine} {formattedMessage.Snippet}";
            var keyboard = _getKeyboardFactory.CreateKeyboard(GetKeyboardState.Minimized, formattedMessage);
            _telegramMethods.SendMessage(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public async Task UpdateMessage(string chatId, int messageId, GetKeyboardState state, FormattedMessage formattedMessage, int page = 0, bool isIgnored = false)
        {
            formattedMessage.NullInspect(nameof(formattedMessage));

            var header = formattedMessage.Header;
            var keyboard = _getKeyboardFactory.CreateKeyboard(state, formattedMessage, page, isIgnored);
            var displayedMessage = page == 0
                ? Emoji.ClosedEmailEnvelop + header + $"{Environment.NewLine}{Environment.NewLine}{formattedMessage.Snippet}"
                : Emoji.RedArrowedEnvelope + header + $"{Environment.NewLine}{Environment.NewLine}{formattedMessage.DesirableBody[page - 1]}";
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
                        MessageText = new SendCallbackData
                        {
                            Command = "deletecommand",
                        }//$"Recipient added:{Environment.NewLine}{contact.Name} <{contact.Email}>"

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

        public async Task<TextMessage> SpecifyNewMailMessage(string chatId, SendKeyboardState state, NmStoreModel model = null)
        {
            var keyboard = _sendKeyboardFactory.CreateKeyboard(state, model);
            var message = BuildNewMailMessage(model);
            return await _telegramMethods.SendMessageAsync(chatId, message, ParseMode.Html, false, false, null, keyboard);
        }

        public async Task SaveAsDraftQuestionMessage(string chatId, SendKeyboardState state)
        {
            var keyboard = _sendKeyboardFactory.CreateKeyboard(state);
            await _telegramMethods.SendMessageAsync(chatId, _storeDraftMessageText, ParseMode.Html, false, false, null, keyboard);
        }

        public async Task UpdateNewMailMessage(string chatId, SendKeyboardState state, NmStoreModel model, string draftId = "")
        {
            var keyboard = _sendKeyboardFactory.CreateKeyboard(state, model, draftId);
            var message = draftId == null 
                ? BuildNewMailMessage(model) 
                : _restoreFromDraftMessageText;
            await _telegramMethods.EditMessageTextAsync(message, chatId, model.MessageId.ToString(), null, ParseMode.Html, null, keyboard);
        }



        public async Task SendAttachmentToChat(string chatId, string fullFileName, string caption)
        {
            await _telegramMethods.SendDocument(chatId, fullFileName, caption);
        }

        public async Task SendLostInfoMessage(string chatId)
        {
            var message = $"{Emoji.WhiteExclamation} Info about this message is lost.";
            await _telegramMethods.SendMessageAsync(chatId, message);
        }

        public async Task NotRecognizedEmailMessage(string chatId, string email)
        {
            var message = $"{Emoji.WhiteExclamation} The address {email} was not recognized.";
            await _telegramMethods.SendMessageAsync(chatId, message);
        }

        public async Task ChangeTextMessageForceReply(string chatId)
        {
            var reply = new ForceReply
            {
                Selective = true
            };
            var message = $"<b>{Commands.MESSAGE_FORCE_REPLY_COMMAND} </b>\r\n{Emoji.InfoSign}<i>To attach files drop them into the chat.</i>";

            await _telegramMethods.SendMessageAsync(chatId, message, ParseMode.Html, false, false, null, reply);
        }

        public async Task ChangeSubjectForceReply(string chatId)
        {
            var reply = new ForceReply
            {
                Selective = true
            };
            var message = $"<b>{Commands.SUBJECT_FORCE_REPLY_COMMAND} </b>";

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

        public async Task SendErrorAboutMaxAttachmentSizeToChat(string chatId)
        {
            throw new NotImplementedException("error");
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

        private string BuildNewMailMessage(NmStoreModel model)
        {
            var message = new StringBuilder(_newMessageMainText);
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
                    builder.Append(Path.GetFileName(item)); //! GetFileName 
                    if (i < collection.Count - 1) builder.Append(", ");
                });
            });
            message.AppendLine();
            iterFunc(message, model.To.Select(a => a.Address).ToList(), "To");
            message.AppendLine();
            iterFunc(message, model.Cc.Select(a => a.Address).ToList(), "Cc");
            message.AppendLine();
            iterFunc(message, model.Bcc.Select(a => a.Address).ToList(), "Bcc");
            message.AppendLine();
            if (model.Subject != null)
                message.AppendLine($"<b>Subject:</b> {model.Subject}");
            if (model.Message != null)
            {
                message.AppendLine("<b>Message:</b>");
                message.AppendLine(model.Message);
            }
            message.AppendLine();
            iterFunc(message, model.File.Select(f => f.OriginalName).ToList(), $"{Emoji.PaperClip}Attachments"); //Emoji probable cause of error, because it will be send inside <b> tag
            return message.ToString();
        }

        private readonly TelegramMethods _telegramMethods;
        private readonly string _newMessageMainText =
                        $"{Emoji.New} Please specify the <b>Recipients</b>, a <b>Subject</b> and the <b>Content</b> of the email: ";

        private readonly string _newMessageTipText = $"{Emoji.InfoSign} You can use quick command, just type in the chat:" +
                        $"{Environment.NewLine}<i>/new \"recipient1@gmail.com, recipient2@gmail.com,...\" \"subject\" \"email text\"</i>" +
                        $"{Environment.NewLine}and press Enter to quick send the email." +
                        $"{Environment.NewLine}{Emoji.InfoSign} For multiple recipients use comma separator.";

        private readonly string _storeDraftMessageText =
            $"{Emoji.QuestionSign} You have already started to create a new message. " +
            $"You can save it as draft and create new instance of new message or continue composing.";

        private readonly string _restoreFromDraftMessageText =
            $"{Emoji.InfoSign} To restore message from draft and continue composing click this button {Emoji.DownArrow}";

        private readonly BotSettings _settings = BotInitializer.Instance.BotSettings;
        private readonly string _contactsThumbUrl;
        private readonly GetKeyboardFactory _getKeyboardFactory = new GetKeyboardFactory();
        private readonly SendKeyboardFactory _sendKeyboardFactory = new SendKeyboardFactory();
    }
}