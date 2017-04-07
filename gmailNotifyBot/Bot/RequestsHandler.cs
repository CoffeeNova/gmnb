using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using CoffeeJelly.gmailNotifyBot.Extensions;
using Newtonsoft.Json.Linq;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    /// <summary>
    /// This class acts as a subscriber <see cref="Requests.RequestsArrivedEvent"/>.
    /// Recognizes Telegram's messages and forms them as <see cref="Message"/> objects.
    /// Triggers self events depending on the message type recieved.
    /// </summary>
    public class RequestsHandler
    {
        public RequestsHandler()
        {
            //if (!_testMode)
            ResumeHandleRequests();
        }

        private void RequestsArrivedEvent(IRequests requests)
        {
            foreach (var request in requests.RequestList)
            {
                dynamic message = MessageBuilder.BuildUnspecifiedMessage(request["message"]);
                #region raising events
                if (message.GetType() == typeof(TextMessage))
                    TelegramTextMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(AudioMessage))
                    TelegramAudioMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(DocumentMessage))
                    TelegramDocumentMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(StickerMessage))
                    TelegramStickerMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(PhotoMessage))
                    TelegramPhotoMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(GameMessage))
                    TelegramGameMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(VideoMessage))
                    TelegramVideoMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(VoiceMessage))
                    TelegramVoiceMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(ContactMessage))
                    TelegramContactMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(LocationMessage))
                    TelegramLocationMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(VenueMessage))
                    TelegramVenueMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(NewChatMemberMessage))
                    TelegramNewChatMemberMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(LeftChatMemberMessage))
                    TelegramLeftChatMemberMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(NewChatTitleMessage))
                    TelegramNewChatTitleMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(NewChatPhotoMessage))
                    TelegramNewChatPhotoMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(MigrateToChatIdMessage))
                    TelegramMigrateToChatIdMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(MigrateFromChatIdMessage))
                    TelegramMigrateFromChatIdMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(PinnedMessage))
                    TelegramPinnedMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(UnknownMessage))
                    TelegramUnknownMessageEvent?.Invoke(message);
                #endregion
            }

        }

        public void StopHandleRequests()
        {
            if (RequestsHandlerStopped) return;
            Requests.RequestsArrivedEvent -= RequestsArrivedEvent;
            RequestsHandlerStopped = true;
        }

        public void ResumeHandleRequests()
        {
            if (!RequestsHandlerStopped) return;
            Requests.RequestsArrivedEvent += RequestsArrivedEvent;
            RequestsHandlerStopped = false;
        }

        #region events

        public delegate void TelegramMessageEventHandler<in T>(T message);

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
        #endregion

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //private bool _anyCommandHandled;
        //private bool _handleConnectCommand;
        //private bool _handleConnectCommand2;


        //private bool AnyCommandHandled
        //{
        //    get { return _anyCommandHandled; }
        //    set
        //    {
        //        if (value)
        //            _anyCommandHandled = true;
        //        if (!value && false.EqualsAll(HandleConnectCommand, _handleConnectCommand2))
        //            _anyCommandHandled = false;
        //    }
        //}

        //public bool HandleConnectCommand
        //{
        //    get { return _handleConnectCommand; }
        //    set
        //    {
        //        if (value != _handleConnectCommand)
        //        {
        //            _handleConnectCommand = value;
        //            AnyCommandHandled = value;
        //            if (value)
        //                AuthorizeCommandEvent += GmnbRequestsHandler_AuthorizeCommandEvent;
        //            else
        //                AuthorizeCommandEvent -= GmnbRequestsHandler_AuthorizeCommandEvent;
        //        }
        //    }
        //}

        //public bool HandleConnectCommand2
        //{
        //    get { return _handleConnectCommand2; }
        //    set
        //    {
        //        if (value != _handleConnectCommand2)
        //        {
        //            _handleConnectCommand2 = value;
        //            if (value)
        //                AuthorizeCommandEvent += GmnbRequestsHandler_AuthorizeCommandEvent;
        //            else
        //                AuthorizeCommandEvent -= GmnbRequestsHandler_AuthorizeCommandEvent;
        //        }
        //    }

        //}

        public bool RequestsHandlerStopped { get; private set; }

        

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