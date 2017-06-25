using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ChosenInlineResultUpdates
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

        public async Task HandleChosenInlineResult(QueryResult.ChosenInlineResult result)
        {
            if (result?.Query == null)
                throw new ArgumentNullException(nameof(result));

            Exception exception = null;
            try
            {
                var service = Methods.SearchServiceByUserId(result.From);
                if (!service.FullUserAccess)
                    return;
                foreach (var rule in _rules)
                {
                    var del = rule.Handle(result, service, this);
                    if (del == null)
                        continue;
                    LogMaker.Log(Logger, $"{result.Query} command received from user with id {result.From.Id}, resultId={result.ResultId}", false);
                    if (result.ResultId == CallbackCommand.IGNORE_COMMAND)
                        return;
                    await del.Invoke();
                }
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
            }
            finally
            {
                if (exception != null)
                    LogMaker.Log(Logger, exception, $"An exception has been thrown in processing InlineQuery with query: {result.Query}");
            }
        }

        private void InitRules()
        {
            _rules.Add(new GetInboxMessagesRule());
            _rules.Add(new GetAllMessagesRule());
            _rules.Add(new GetToContactsRule());
            _rules.Add(new GetCcContactsRule());
            _rules.Add(new GetBccContactsRule());
            _rules.Add(new GetDraftMessagesRule());
            _rules.Add(new GetEditDraftRule());
        }

        private readonly List<IChosenInlineResultHandlerRules> _rules = new List<IChosenInlineResultHandlerRules>();
        private readonly BotActions _botActions;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GmailDbContextWorker _dbWorker;
    }
}