﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Implemented using the rules pattern.</remarks>
    public partial class MessageHandler
    {
        public MessageHandler()
        {
            try
            {
                _authorizer = BotInitializer.Instance.Authorizer;
                _dbWorker = new GmailDbContextWorker();
                _botSettings = BotInitializer.Instance.BotSettings;
                _botActions = new BotActions(_botSettings.Token);
                InitRules();
                InitForceReplyRules();
                BotInitializer.Instance.UpdatesHandler.TelegramTextMessageEvent += HandleTextMessage;
                BotInitializer.Instance.UpdatesHandler.TelegramTextMessageEvent += HandleForceReplyMessage;
                BotInitializer.Instance.UpdatesHandler.TelegramDocumentMessageEvent += HandleForceReplyMessage;
                BotInitializer.Instance.UpdatesHandler.TelegramAudioMessageEvent += HandleForceReplyMessage;
                BotInitializer.Instance.UpdatesHandler.TelegramVideoMessageEvent += HandleForceReplyMessage;
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(MessageHandler), ex);
            }
        }

        private async void HandleForceReplyMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            var reply = message.ReplyToMessage as TextMessage;
            if (reply == null)
                return;

            foreach (var rule in _forceReplyRules)
            {
                var rate = rule.Handle(message, this);
                if (rate == null) continue;
                Exception exception = null;
                LogMaker.Log(Logger, $"File received from user with id {(string)message.From}", false);
                try
                {
                    await rate.Invoke(message);
                }
                catch (ServiceNotFoundException ex)
                {
                    exception = ex;
                    await _botActions.WrongCredentialsMessage(message.From);
                }
                catch (DbDataStoreException ex)
                {
                    exception = ex;
                    await _botActions.WrongCredentialsMessage(message.From);
                }
                catch (AuthorizeException ex)
                {
                    exception = ex;
                    await _botActions.AuthorizationErrorMessage(message.From);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    Debug.Assert(false, "operation error show to telegram chat as answerCallbackQuery");
                }
                finally
                {
                    if (exception != null)
                        LogMaker.Log(Logger, exception,
                            $"An exception has been thrown in processing Message with ReplyToMessage.Text: {reply.Text}");
                    try
                    {
                        await _botActions.RemoveKeyboard(message.From);
                    }
                    catch (Exception ex)
                    {
                        LogMaker.Log(Logger, ex,
                               $"An exception has been thrown in attempt to send a request to remove keyboard.");
                    }
                }
            }
        }

        private async void HandleTextMessage(TextMessage message)
        {
            if (message?.Text == null)
                throw new ArgumentNullException(nameof(message));
            if (message.ReplyToMessage != null) return;

            foreach (var rule in _rules)
            {
                var rate = rule.Handle(message, this);
                if (rate == null) continue;
                Exception exception = null;
                LogMaker.Log(Logger, $"{message.Text} command received from user with id {(string)message.From}", false);
                try
                {
                    await rate.Invoke(message);
                }
                catch (ServiceNotFoundException ex)
                {
                    exception = ex;
                    await _botActions.WrongCredentialsMessage(message.From);
                }
                catch (DbDataStoreException ex)
                {
                    exception = ex;
                    await _botActions.WrongCredentialsMessage(message.From);
                }
                catch (AuthorizeException ex)
                {
                    exception = ex;
                    await _botActions.AuthorizationErrorMessage(message.From);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    Debug.Assert(false, "operation error show to telegram chat as answerCallbackQuery");
                }
                finally
                {
                    if (exception != null)
                        LogMaker.Log(Logger, exception,
                            $"An exception has been thrown in processing TextMessage with text: {message.Text}");
                }
            }
        }

        private void InitRules()
        {
            _rules.Add(new AuthorizeRule());
            _rules.Add(new TestMessageRule());
            _rules.Add(new TestNameRule());
            _rules.Add(new TestThreadRule());
            _rules.Add(new StartNotifyRule());
            _rules.Add(new StopNotifyRule());
            _rules.Add(new StartWatchRule());
            _rules.Add(new StopWatchRule());
            _rules.Add(new NewMessageRule());
            _rules.Add(new GetInboxRule());
            _rules.Add(new GetAllRule());
            _rules.Add(new GetDraftRule());
            _rules.Add(new TestDraftRule());
            _rules.Add(new ShowSettingsRule());

        }

        private void InitForceReplyRules()
        {
            _forceReplyRules.Add(new MessageForceReplyRule());
            _forceReplyRules.Add(new SubjectForceReplyRule());
            _forceReplyRules.Add(new FileForceReplyRule());
            #region Settings
            _forceReplyRules.Add(new CreateNewLabelForceReplyRule());
            _forceReplyRules.Add(new EditLabelNameForceReplyRule());
            _forceReplyRules.Add(new AddToIgnoreForceReplyRule());
            _forceReplyRules.Add(new RemoveFromIgnoreForceReplyRule());
            #endregion
        }

        private readonly List<IMessageHandlerRules> _rules = new List<IMessageHandlerRules>();
        private readonly List<IMessageHandlerRules> _forceReplyRules = new List<IMessageHandlerRules>();
        private readonly BotActions _botActions;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GmailDbContextWorker _dbWorker;
        private readonly BotSettings _botSettings;
        private readonly Authorizer _authorizer;
    }

}