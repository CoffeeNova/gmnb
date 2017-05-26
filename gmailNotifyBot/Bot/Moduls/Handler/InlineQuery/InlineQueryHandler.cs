using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.Handler.CallbackQuery;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.Handler.InlineQuery
{
    using Query = TelegramBotApiWrapper.Types.InlineQuery;
    public partial class InlineQueryHandler
    {
        public InlineQueryHandler()
        {
            _dbWorker = new GmailDbContextWorker();
            _botActions = new BotActions(BotInitializer.Instance.BotSettings.Token);
            InitRules();
            BotInitializer.Instance.UpdatesHandler.TelegramInlineQueryEvent += HandleInlineQuery;
        }

        public async void HandleInlineQuery(Query query)
        {
            if (query?.Query == null)
                throw new ArgumentNullException(nameof(query));

            foreach (var rule in _rules)
            {
                var rate = rule.Handle(this);
                if (rate == null) continue;

                Exception exception = null;
                LogMaker.Log(Logger, $"{query.Query} command received from user with id {query.From}", false);
                try
                {
                    await rate.Invoke(query);
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
            _rules.Add();
        }

        private readonly List<IInlineQueryHandlerRules> _rules = new List<IInlineQueryHandlerRules>();
        private readonly BotActions _botActions;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GmailDbContextWorker _dbWorker;
    }
}