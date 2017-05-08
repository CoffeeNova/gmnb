﻿using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
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
                TryRaiseNewInlineQueryEvent(update.InlineQuery);
                TryRaiseNewChosenInlineResult(update.ChosenInlineResult);
            }
        }

        private void TryRaiseNewMessageEvent(Message sender)
        {
            if (sender == null) return;

            dynamic dSender = sender;

            #region raising Message events
            if (dSender.GetType() == typeof(TextMessage))
                TelegramTextMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(AudioMessage))
                TelegramAudioMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(DocumentMessage))
                TelegramDocumentMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(StickerMessage))
                TelegramStickerMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(PhotoMessage))
                TelegramPhotoMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(GameMessage))
                TelegramGameMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(VideoMessage))
                TelegramVideoMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(VoiceMessage))
                TelegramVoiceMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(ContactMessage))
                TelegramContactMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(LocationMessage))
                TelegramLocationMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(VenueMessage))
                TelegramVenueMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(NewChatMemberMessage))
                TelegramNewChatMemberMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(LeftChatMemberMessage))
                TelegramLeftChatMemberMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(NewChatTitleMessage))
                TelegramNewChatTitleMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(NewChatPhotoMessage))
                TelegramNewChatPhotoMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(MigrateToChatIdMessage))
                TelegramMigrateToChatIdMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(MigrateFromChatIdMessage))
                TelegramMigrateFromChatIdMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(PinnedMessage))
                TelegramPinnedMessageEvent?.Invoke(dSender);

            else if (dSender.GetType() == typeof(UnknownMessage))
                TelegramUnknownMessageEvent?.Invoke(dSender);
            #endregion
        }

        private void TryRaiseNewCallbackQueryEvent(CallbackQuery sender)
        {
            if (sender == null) return;
            TelegramCallbackQueryEvent?.Invoke(sender);
        }

        private void TryRaiseNewInlineQueryEvent(InlineQuery sender)
        {
            if (sender == null) return;
            TelegramInlineQueryEvent?.Invoke(sender);
        }

        private void TryRaiseNewChosenInlineResult(ChosenInlineResult sender)
        {
            if (sender == null) return;
            TelegramChosenInlineEvent?.Invoke(sender);
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
        public delegate void TelegramSenderEventHandler<in T>(T sender) where T : ISender;

        public event TelegramSenderEventHandler<TextMessage> TelegramTextMessageEvent;
        public event TelegramSenderEventHandler<AudioMessage> TelegramAudioMessageEvent;
        public event TelegramSenderEventHandler<DocumentMessage> TelegramDocumentMessageEvent;
        public event TelegramSenderEventHandler<StickerMessage> TelegramStickerMessageEvent;
        public event TelegramSenderEventHandler<PhotoMessage> TelegramPhotoMessageEvent;
        public event TelegramSenderEventHandler<GameMessage> TelegramGameMessageEvent;
        public event TelegramSenderEventHandler<VideoMessage> TelegramVideoMessageEvent;
        public event TelegramSenderEventHandler<VoiceMessage> TelegramVoiceMessageEvent;
        public event TelegramSenderEventHandler<ContactMessage> TelegramContactMessageEvent;
        public event TelegramSenderEventHandler<LocationMessage> TelegramLocationMessageEvent;
        public event TelegramSenderEventHandler<VenueMessage> TelegramVenueMessageEvent;
        public event TelegramSenderEventHandler<NewChatMemberMessage> TelegramNewChatMemberMessageEvent;
        public event TelegramSenderEventHandler<LeftChatMemberMessage> TelegramLeftChatMemberMessageEvent;
        public event TelegramSenderEventHandler<NewChatTitleMessage> TelegramNewChatTitleMessageEvent;
        public event TelegramSenderEventHandler<NewChatPhotoMessage> TelegramNewChatPhotoMessageEvent;
        public event TelegramSenderEventHandler<MigrateToChatIdMessage> TelegramMigrateToChatIdMessageEvent;
        public event TelegramSenderEventHandler<MigrateFromChatIdMessage> TelegramMigrateFromChatIdMessageEvent;
        public event TelegramSenderEventHandler<PinnedMessage> TelegramPinnedMessageEvent;
        public event TelegramSenderEventHandler<UnknownMessage> TelegramUnknownMessageEvent;
        public event TelegramSenderEventHandler<CallbackQuery> TelegramCallbackQueryEvent;
        public event TelegramSenderEventHandler<InlineQuery> TelegramInlineQueryEvent;
        public event TelegramSenderEventHandler<ChosenInlineResult> TelegramChosenInlineEvent;
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