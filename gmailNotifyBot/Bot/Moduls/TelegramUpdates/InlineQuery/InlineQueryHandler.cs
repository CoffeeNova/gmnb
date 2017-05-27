using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.InlineQuery
{
    using Query = TelegramBotApiWrapper.Types.InlineQuery;
    public partial class InlineQueryHandler
    {
        public InlineQueryHandler(string token, UpdatesHandler updatesHandler)
        {
            token.NullInspect(nameof(token));
            updatesHandler.NullInspect(nameof(updatesHandler));

            _botActions = new BotActions(token);
            InitRules();
            updatesHandler.TelegramInlineQueryEvent += HandleInlineQuery;
        }

        public async void HandleInlineQuery(Query query)
        {
            if (query?.Query == null)
                throw new ArgumentNullException(nameof(query));

            foreach (var rule in _rules)
            {
                var del = rule.Handle(query, this);
                if (del == null) continue;

                Exception exception = null;
                LogMaker.Log(Logger, $"{query.Query} command received from user with id {query.From}", false);
                try
                {
                    await del.Invoke();
                }
                catch (ServiceNotFoundException ex)
                {
                    exception = ex;
                    await _botActions.WrongCredentialsMessage(query.From);
                }
                catch (DbDataStoreException ex)
                {
                    exception = ex;
                    await _botActions.WrongCredentialsMessage(query.From);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    Debug.Assert(false, "operation error show to telegram chat as answerCallbackQuery");
                }
                finally
                {
                    if (exception != null)
                        LogMaker.Log(Logger, exception, $"An exception has been thrown in processing InlineQuery with command {query.Query}");
                }
            }
        }

        private void InitRules()
        {
            _rules.Add(new ShowInboxMessagesRule());
            _rules.Add(new ShowAllMessagesRule());
            _rules.Add(new ShowContactsRule());
        }

        private readonly List<IInlineQueryHandlerRules> _rules = new List<IInlineQueryHandlerRules>();
        private readonly BotActions _botActions;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    }
}