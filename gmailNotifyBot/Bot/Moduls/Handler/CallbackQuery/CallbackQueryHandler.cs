using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.Handler.Message;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.Handler.CallbackQuery
{
    using Query = TelegramBotApiWrapper.Types.General.CallbackQuery;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Implemented using the rules pattern.</remarks>
    public partial class CallbackQueryHandler
    {
        public CallbackQueryHandler()
        {
            if (BotInitializer.Instance == null)
                throw new CommandHandlerException($"Initialize exception. {nameof(BotInitializer)} must be initialized.");
            if (BotInitializer.Instance.BotSettings == null)
                throw new CommandHandlerException($"Initialize exception. {nameof(BotInitializer.BotSettings)} must be initialized.");
            if (BotInitializer.Instance.UpdatesHandler == null)
                throw new CommandHandlerException($"Initialize exception. {nameof(BotInitializer.UpdatesHandler)} must be initialized.");
            if (BotInitializer.Instance.UpdatesHandler == null)
                throw new CommandHandlerException($"Initialize exception. {nameof(BotInitializer.UpdatesHandler)} must be initialized.");
            if(string.IsNullOrEmpty(BotInitializer.Instance.BotSettings.Token))
                throw new CommandHandlerException($"Initialize exception. {nameof(BotInitializer.Instance.BotSettings.Token)} must be not null or empty.");
            if (string.IsNullOrEmpty(BotInitializer.Instance.BotSettings.Topic))
                throw new CommandHandlerException($"Initialize exception. {nameof(BotInitializer.Instance.BotSettings.Topic)} must be not null or empty.");

            _dbWorker = new GmailDbContextWorker();
            _botActions = new BotActions(BotInitializer.Instance.BotSettings.Token);
            InitRules();
            BotInitializer.Instance.UpdatesHandler.TelegramCallbackQueryEvent += Handle;

        }

        public async void Handle(Query query)
        {
            if (query?.Data == null)
                throw new ArgumentNullException(nameof(query));

            var data = new CallbackData(query.Data);

            foreach (var rule in _rules)
            {
                var rate = rule.Handle(data, this);
                if (rate == null) continue;

                Exception exception = null;
                LogMaker.Log(Logger, $"{query.Data} command received from user with id {(string)query.From}", false);
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
                catch (AuthorizeException ex)
                {
                    exception = ex;
                    await _botActions.AuthorizationErrorMessage(query.From);
                }
                catch (Exception ex)
                {
                    exception = ex;
                    Debug.Assert(false, "operation error show to telegram chat as answerCallbackQuery");
                }
                finally
                {
                    if (exception != null)
                        LogMaker.Log(Logger, exception, $"An exception has been thrown in processing CallbackQuery with command {query.Data}");
                }
            }
        }

        private void InitRules()
        {
            _rules.Add(new SendAuthorizeLinkRule());
            _rules.Add(new ExpandRule());
            _rules.Add(new HideRule());
            _rules.Add(new ExpandActionsRule());
            _rules.Add(new HideActionsRule());
            _rules.Add(new ToReadRule());
            _rules.Add(new ToUnReadRule());
            _rules.Add(new ToSpamRule());
            _rules.Add(new ToInboxRule());
            _rules.Add(new ToTrashRule());
            _rules.Add(new ArchiveRule());
            _rules.Add(new UnignoreRule());
            _rules.Add(new IgnoreRule());
            _rules.Add(new NextPageRule());
            _rules.Add(new PrevPageRule());
            _rules.Add(new AddSubjectRule());
            _rules.Add(new GetAttachmentsRule());
            _rules.Add(new HideAttachmentsRule());
        }

       
        private readonly List<ICallbackQueryHandlerRules> _rules = new List<ICallbackQueryHandlerRules>();
        private readonly BotActions _botActions;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GmailDbContextWorker _dbWorker;
    }

}