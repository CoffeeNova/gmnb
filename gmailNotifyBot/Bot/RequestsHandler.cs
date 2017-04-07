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

                else if (message.GetType() == typeof(TelegramAudioMessage))
                    TelegramAudioMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramDocumentMessage))
                    TelegramDocumentMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramStickerMessage))
                    TelegramStickerMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramPhotoMessage))
                    TelegramPhotoMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramGameMessage))
                    TelegramGameMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramVideoMessage))
                    TelegramVideoMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramVoiceMessage))
                    TelegramVoiceMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramContactMessage))
                    TelegramContactMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramLocationMessage))
                    TelegramLocationMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramVenueMessage))
                    TelegramVenueMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramNewChatMemberMessage))
                    TelegramNewChatMemberMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramLeftChatMemberMessage))
                    TelegramLeftChatMemberMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramNewChatTitleMessage))
                    TelegramNewChatTitleMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramNewChatPhotoMessage))
                    TelegramNewChatPhotoMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramMigrateToChatIdMessage))
                    TelegramMigrateToChatIdMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramMigrateFromChatIdMessage))
                    TelegramMigrateFromChatIdMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramPinnedMessage))
                    TelegramPinnedMessageEvent?.Invoke(message);

                else if (message.GetType() == typeof(TelegramUnknownMessage))
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
        public event TelegramMessageEventHandler<TelegramAudioMessage> TelegramAudioMessageEvent;
        public event TelegramMessageEventHandler<TelegramDocumentMessage> TelegramDocumentMessageEvent;
        public event TelegramMessageEventHandler<TelegramStickerMessage> TelegramStickerMessageEvent;
        public event TelegramMessageEventHandler<TelegramPhotoMessage> TelegramPhotoMessageEvent;
        public event TelegramMessageEventHandler<TelegramGameMessage> TelegramGameMessageEvent;
        public event TelegramMessageEventHandler<TelegramVideoMessage> TelegramVideoMessageEvent;
        public event TelegramMessageEventHandler<TelegramVoiceMessage> TelegramVoiceMessageEvent;
        public event TelegramMessageEventHandler<TelegramContactMessage> TelegramContactMessageEvent;
        public event TelegramMessageEventHandler<TelegramLocationMessage> TelegramLocationMessageEvent;
        public event TelegramMessageEventHandler<TelegramVenueMessage> TelegramVenueMessageEvent;
        public event TelegramMessageEventHandler<TelegramNewChatMemberMessage> TelegramNewChatMemberMessageEvent;
        public event TelegramMessageEventHandler<TelegramLeftChatMemberMessage> TelegramLeftChatMemberMessageEvent;
        public event TelegramMessageEventHandler<TelegramNewChatTitleMessage> TelegramNewChatTitleMessageEvent;
        public event TelegramMessageEventHandler<TelegramNewChatPhotoMessage> TelegramNewChatPhotoMessageEvent;
        public event TelegramMessageEventHandler<TelegramMigrateToChatIdMessage> TelegramMigrateToChatIdMessageEvent;
        public event TelegramMessageEventHandler<TelegramMigrateFromChatIdMessage> TelegramMigrateFromChatIdMessageEvent;
        public event TelegramMessageEventHandler<TelegramPinnedMessage> TelegramPinnedMessageEvent;
        public event TelegramMessageEventHandler<TelegramUnknownMessage> TelegramUnknownMessageEvent;
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