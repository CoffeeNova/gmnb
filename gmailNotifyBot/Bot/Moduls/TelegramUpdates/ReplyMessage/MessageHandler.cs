using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using NLog;
using MessageHandler = CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.MessageUpdates.MessageHandler;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ReplyMessage
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Implemented using the rules pattern.</remarks>
    public partial class ReplyMessageHandler
    {
        public ReplyMessageHandler()
        {
            try
            {
                _authorizer = BotInitializer.Instance.Authorizer;
                _dbWorker = new GmailDbContextWorker();
                _botSettings = BotInitializer.Instance.BotSettings;
                _botActions = new BotActions(_botSettings.Token);
                InitRules();
                BotInitializer.Instance.UpdatesHandler.TelegramTextMessageEvent += Handle;
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(MessageHandler), ex);
            }
        }

        public async void Handle(DocumentMessage message)
        {
            if (message?.Document == null)
                throw new ArgumentNullException(nameof(message.Document));

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

        }

        private readonly List<IDocMessageRules> _rules = new List<IDocMessageRules>();
        private readonly BotActions _botActions;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GmailDbContextWorker _dbWorker;
        private readonly BotSettings _botSettings;
        private readonly Authorizer _authorizer;
    }

}