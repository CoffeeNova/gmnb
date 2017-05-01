using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram.JsonParsers;
using CoffeeJelly.gmailNotifyBot.Extensions;
using Newtonsoft.Json.Linq;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    /// <summary>
    /// This class acts as a subscriber <see cref="Updates.UpdatesArrivedEvent"/>.
    /// Recognizes Telegram's messages and forms them as <see cref="Message"/> objects.
    /// Triggers self events depending on the message type recieved.
    /// </summary>
    public class UpdatesHandler
    {
        public UpdatesHandler()
        {
            //if (!_testMode)
            ResumeHandleUpdates();
        }

        private void RequestsArrivedEvent(IUpdates updates)
        {
            foreach (var update in updates.UpdatesList)
            {
                TryRaiseNewMessageEvent(update.Message);
                TryRaiseNewCallbackQueryEvent(update.CallbackQuery);
            }

        }

        private void TryRaiseNewMessageEvent(Message message)
        {
            if (message == null) return;

            dynamic dMessage = message;

            #region raising new message events
            if (dMessage.GetType() == typeof(TextMessage))
                TelegramTextMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(AudioMessage))
                TelegramAudioMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(DocumentMessage))
                TelegramDocumentMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(StickerMessage))
                TelegramStickerMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(PhotoMessage))
                TelegramPhotoMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(GameMessage))
                TelegramGameMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(VideoMessage))
                TelegramVideoMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(VoiceMessage))
                TelegramVoiceMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(ContactMessage))
                TelegramContactMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(LocationMessage))
                TelegramLocationMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(VenueMessage))
                TelegramVenueMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(NewChatMemberMessage))
                TelegramNewChatMemberMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(LeftChatMemberMessage))
                TelegramLeftChatMemberMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(NewChatTitleMessage))
                TelegramNewChatTitleMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(NewChatPhotoMessage))
                TelegramNewChatPhotoMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(MigrateToChatIdMessage))
                TelegramMigrateToChatIdMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(MigrateFromChatIdMessage))
                TelegramMigrateFromChatIdMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(PinnedMessage))
                TelegramPinnedMessageEvent?.Invoke(dMessage);

            else if (dMessage.GetType() == typeof(UnknownMessage))
                TelegramUnknownMessageEvent?.Invoke(dMessage);
            #endregion

        }

        private void TryRaiseNewCallbackQueryEvent(CallbackQuery callbackQuery)
        {
            if (callbackQuery == null) return;
            TelegramCallbackQueryEvent?.Invoke(callbackQuery);
        }


        public void StopHandleUpdates()
        {
            if (UpdatesHandlerStopped) return;
            Updates.Instance.UpdatesArrivedEvent -= RequestsArrivedEvent;
            UpdatesHandlerStopped = true;
        }

        public void ResumeHandleUpdates()
        {
            if (!UpdatesHandlerStopped) return;
            Updates.Instance.UpdatesArrivedEvent += RequestsArrivedEvent;
            UpdatesHandlerStopped = false;
        }

        #region events

        public delegate void TelegramMessageEventHandler<in T>(T message);

        public delegate void TelegramCallbackQuery(CallbackQuery callbackQuery);

        public event TelegramMessageEventHandler<TextMessage> TelegramTextMessageEvent;
        public event TelegramMessageEventHandler<AudioMessage> TelegramAudioMessageEvent;
        public event TelegramMessageEventHandler<DocumentMessage> TelegramDocumentMessageEvent;
        public event TelegramMessageEventHandler<StickerMessage> TelegramStickerMessageEvent;
        public event TelegramMessageEventHandler<PhotoMessage> TelegramPhotoMessageEvent;
        public event TelegramMessageEventHandler<GameMessage> TelegramGameMessageEvent;
        public event TelegramMessageEventHandler<VideoMessage> TelegramVideoMessageEvent;
        public event TelegramMessageEventHandler<VoiceMessage> TelegramVoiceMessageEvent;
        public event TelegramMessageEventHandler<ContactMessage> TelegramContactMessageEvent;
        public event TelegramMessageEventHandler<LocationMessage> TelegramLocationMessageEvent;
        public event TelegramMessageEventHandler<VenueMessage> TelegramVenueMessageEvent;
        public event TelegramMessageEventHandler<NewChatMemberMessage> TelegramNewChatMemberMessageEvent;
        public event TelegramMessageEventHandler<LeftChatMemberMessage> TelegramLeftChatMemberMessageEvent;
        public event TelegramMessageEventHandler<NewChatTitleMessage> TelegramNewChatTitleMessageEvent;
        public event TelegramMessageEventHandler<NewChatPhotoMessage> TelegramNewChatPhotoMessageEvent;
        public event TelegramMessageEventHandler<MigrateToChatIdMessage> TelegramMigrateToChatIdMessageEvent;
        public event TelegramMessageEventHandler<MigrateFromChatIdMessage> TelegramMigrateFromChatIdMessageEvent;
        public event TelegramMessageEventHandler<PinnedMessage> TelegramPinnedMessageEvent;
        public event TelegramMessageEventHandler<UnknownMessage> TelegramUnknownMessageEvent;
        public event TelegramCallbackQuery TelegramCallbackQueryEvent;
        #endregion

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public bool UpdatesHandlerStopped { get; private set; } = true;



    }

    public enum RequestsType
    {
        Text,
        Sticker,
        Photo,
        Video,
        Document,
        Location,
        Contact,
        Voice
    }
}