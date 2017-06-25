using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
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
                InitFullAccessRules();
                InitNotifyAccessRules();
                InitFullForceReplyRules();
                InitNotifyForceReplyRules();
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

        private async Task HandleForceReplyMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            var reply = message.ReplyToMessage as TextMessage;
            if (reply == null)
                return;

            Exception exception = null;
            try
            {
                var service = Methods.SearchServiceByUserId(message.From);
                var userSettings = await _dbWorker.FindUserSettingsAsync(message.From);
                if (userSettings == null)
                    throw new DbDataStoreException(
                    $"Can't find user settings data in database. User record with id {message.From.Id} is absent in the database.");

                var rules = userSettings.Access == UserAccess.FULL
                    ? _fullForceReplyRules
                    : _notifyForceReplyRules;

                foreach (var rule in rules)
                {
                    var rate = rule.Handle(message, service, this);
                    if (rate == null) continue;

                    LogMaker.Log(Logger, $"File received from user with id {message.From.Id}", false);
                    await rate.Invoke(message);
                }
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

        private async Task HandleTextMessage(TextMessage message)
        {
            if (message?.Text == null)
                throw new ArgumentNullException(nameof(message));
            if (message.ReplyToMessage != null)
                return;

            Exception exception = null;
            try
            {
                var resultInvoke = await RuleInvokeAction(_startRule, message);
                if (resultInvoke) return;
                resultInvoke = await RuleInvokeAction(_authRule, message);
                if (resultInvoke) return;

                var service = Methods.SearchServiceByUserId(message.From);
                var rules = service.FullUserAccess
                     ? _fullAccessRules
                     : _notifyAccessRules;
                foreach (var rule in rules)
                    await RuleInvokeAction(rule, message, service);
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
            }
            finally
            {
                if (exception != null)
                    LogMaker.Log(Logger, exception,
                        $"An exception has been thrown in processing TextMessage with text: {message.Text}");
            }
        }

        private async Task<bool> RuleInvokeAction(IMessageHandlerRule rule, TextMessage message,
            Service service = null)
        {
            var rate = rule.Handle(message, service, this);
            if (rate == null)
                return false;
            LogMaker.Log(Logger, $"{message.Text} command received from user with id {message.From.Id}", false);
            await rate.Invoke(message);
            return true;
        }

        private void InitFullAccessRules()
        {
            _fullAccessRules.Add(new TestMessageRule());
            _fullAccessRules.Add(new TestNameRule());
            _fullAccessRules.Add(new TestThreadRule());
            _fullAccessRules.Add(new StartNotifyRule());
            _fullAccessRules.Add(new StopNotifyRule());
#if DEBUG
            _fullAccessRules.Add(new StartWatchRule());
            _fullAccessRules.Add(new StopWatchRule());
#endif
            _fullAccessRules.Add(new NewMessageRule());
            _fullAccessRules.Add(new HelpRule());
            _fullAccessRules.Add(new TestDraftRule());
            _fullAccessRules.Add(new ShowSettingsRule());
        }

        private void InitNotifyAccessRules()
        {
            _notifyAccessRules.Add(new TestMessageRule());
            _notifyAccessRules.Add(new TestNameRule());
            _notifyAccessRules.Add(new TestThreadRule());
            _notifyAccessRules.Add(new StartNotifyRule());
            _notifyAccessRules.Add(new StopNotifyRule());
#if DEBUG
            _notifyAccessRules.Add(new StartWatchRule());
            _notifyAccessRules.Add(new StopWatchRule());
#endif
            _notifyAccessRules.Add(new HelpRule());
            _notifyAccessRules.Add(new ShowSettingsRule());
        }

        private void InitFullForceReplyRules()
        {
            _fullForceReplyRules.Add(new MessageForceReplyRule());
            _fullForceReplyRules.Add(new SubjectForceReplyRule());
            _fullForceReplyRules.Add(new FileForceReplyRule());
            #region Settings
            _fullForceReplyRules.Add(new CreateNewLabelForceReplyRule());
            _fullForceReplyRules.Add(new EditLabelNameForceReplyRule());
            _fullForceReplyRules.Add(new AddToIgnoreForceReplyRule());
            _fullForceReplyRules.Add(new RemoveFromIgnoreForceReplyRule());
            #endregion
        }

        private void InitNotifyForceReplyRules()
        {
            #region settings
            _notifyForceReplyRules.Add(new AddToIgnoreForceReplyRule());
            _notifyForceReplyRules.Add(new RemoveFromIgnoreForceReplyRule());
            #endregion
        }

        private readonly IMessageHandlerRule _authRule = new AuthorizeRule();
        private readonly IMessageHandlerRule _startRule = new StartRule();
        private readonly List<IMessageHandlerRule> _fullAccessRules = new List<IMessageHandlerRule>();
        private readonly List<IMessageHandlerRule> _notifyAccessRules = new List<IMessageHandlerRule>();
        private readonly List<IMessageHandlerRule> _fullForceReplyRules = new List<IMessageHandlerRule>();
        private readonly List<IMessageHandlerRule> _notifyForceReplyRules = new List<IMessageHandlerRule>();
        private readonly BotActions _botActions;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GmailDbContextWorker _dbWorker;
        private readonly BotSettings _botSettings;
        private readonly Authorizer _authorizer;
    }

}