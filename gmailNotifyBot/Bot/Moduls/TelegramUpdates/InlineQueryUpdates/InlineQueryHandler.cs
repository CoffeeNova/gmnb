using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQueryUpdates;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.InlineQueryUpdates
{
    using Query = TelegramBotApiWrapper.Types.InlineQuery;
    public partial class InlineQueryHandler
    {
        public InlineQueryHandler()
        {
            try
            {
                _botSettings = BotInitializer.Instance.BotSettings;
                _botActions = new BotActions(_botSettings.Token);
                _dbWorker = new GmailDbContextWorker();
                InitFullAccessRules();
                InitNotifyAccessRules();
                BotInitializer.Instance.UpdatesHandler.TelegramInlineQueryEvent += HandleInlineQuery;
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(InlineQueryHandler), ex);
            }
        }

        public async Task HandleInlineQuery(Query query)
        {
            if (query?.Query == null)
                throw new ArgumentNullException(nameof(query));

            Exception exception = null;
            try
            {
                var service = Methods.SearchServiceByUserId(query.From);

                var rules = service.FullUserAccess
                    ? _fullAccessRules
                    : _notifyAccessRules;
                foreach (var rule in rules)
                {
                    var del = rule.Handle(query, this);
                    if (del == null)
                        continue;
                    LogMaker.Log(Logger, $"{query.Query} command received from user with id {query.From.Id}", false);
                    await del.Invoke();
                }
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
            }
            finally
            {
                if (exception != null)
                    LogMaker.Log(Logger, exception, $"An exception has been thrown in processing InlineQuery with query: {query.Query}");
            }
        }

        private void InitFullAccessRules()
        {
            _fullAccessRules.Add(new ShowInboxMessagesRule());
            _fullAccessRules.Add(new ShowAllMessagesRule());
            _fullAccessRules.Add(new ShowToContactsRule());
            _fullAccessRules.Add(new ShowCcContactsRule());
            _fullAccessRules.Add(new ShowBccContactsRule());
            _fullAccessRules.Add(new ShowDraftMessagesRule());
            _fullAccessRules.Add(new ShowEditDraftMessagesRule());
        }

        private void InitNotifyAccessRules()
        {
        }

        private readonly List<IInlineQueryHandlerRules> _fullAccessRules = new List<IInlineQueryHandlerRules>();
        private readonly List<IInlineQueryHandlerRules> _notifyAccessRules = new List<IInlineQueryHandlerRules>();
        private readonly BotActions _botActions;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly BotSettings _botSettings;
        private readonly GmailDbContextWorker _dbWorker;
    }
}