using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ChosenInlineResult
{
    using QueryResult = TelegramBotApiWrapper.Types.InlineQueryResult;
    public partial class ChosenInlineResultHandler
    {
        public ChosenInlineResultHandler()
        {
            try
            {
                _botActions = new BotActions(BotInitializer.Instance.BotSettings.Token);
                InitRules();
                _dbWorker = new GmailDbContextWorker();
                BotInitializer.Instance.UpdatesHandler.TelegramChosenInlineEvent += HandleChosenInlineResult;
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(ChosenInlineResultHandler), ex);
            }
        }

        public async void HandleChosenInlineResult(QueryResult.ChosenInlineResult result)
        {
            if (result?.Query == null)
                throw new ArgumentNullException(nameof(result));

            foreach (var rule in _rules)
            {
                var del = rule.Handle(result, this);
                if (del == null) continue;

                Exception exception = null;
                LogMaker.Log(Logger, $"{result.Query} command received from user with id {(string)result.From}", false);
                try
                {
                    await del.Invoke();
                }
                catch (ServiceNotFoundException ex)
                {
                    exception = ex;
                    await _botActions.WrongCredentialsMessage(result.From);
                }
                catch (DbDataStoreException ex)
                {
                    exception = ex;
                    await _botActions.WrongCredentialsMessage(result.From);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    Debug.Assert(false, "operation error show to telegram chat as answerCallbackQuery");
                }
                finally
                {
                    if (exception != null)
                        LogMaker.Log(Logger, exception, $"An exception has been thrown in processing InlineQuery with command {result.Query}");
                }
            }
        }

        private void InitRules()
        {
            _rules.Add(new GetInboxMessagesRule());
            _rules.Add(new GetAllMessagesRule());
            _rules.Add(new GetToContactsRule());
            _rules.Add(new GetCcContactsRule());
            _rules.Add(new GetBccContactsRule());
        }

        private readonly List<IChosenInlineResultHandlerRules> _rules = new List<IChosenInlineResultHandlerRules>();
        private readonly BotActions _botActions;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GmailDbContextWorker _dbWorker;
    }
}