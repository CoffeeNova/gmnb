using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.Message
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Implemented using the rules pattern.</remarks>
    public partial class MessageHandler
    {
        public MessageHandler(string token, UpdatesHandler updatesHandler)
        {
            token.NullInspect(nameof(token));
            updatesHandler.NullInspect(nameof(updatesHandler));

            _dbWorker = new GmailDbContextWorker();
            _botActions = new BotActions(token);
            InitRules();
            updatesHandler.TelegramTextMessageEvent += Handle;
        }

        public async void Handle(TextMessage message)
        {
            if (message?.Text == null)
                throw new ArgumentNullException(nameof(message));

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
                            $"An exception has been thrown in processing TextMessage with command {message.Text}");
                }
            }
        }

        private void InitRules()
        {
            _rules.Add(new TestMessageRule());
            _rules.Add(new TestNameRule());
            _rules.Add(new TestThreadRule());
            _rules.Add(new StartNotifyRule());
            _rules.Add(new StopNotifyRule());
            _rules.Add(new StartWatchRule());
            _rules.Add(new StopWatchRule());
            _rules.Add(new NewMessageRule());
            _rules.Add(new GetInboxRule());
            _rules.Add(new GetAddTextMessageRule());
        }

        private readonly List<IMessageHandlerRules> _rules = new List<IMessageHandlerRules>();
        private readonly BotActions _botActions;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GmailDbContextWorker _dbWorker;
    }

}