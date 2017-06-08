using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards;
using CoffeeJelly.gmailNotifyBot.Bot.Moduls.GoogleRequests;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Exceptions;
using NLog;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.CallbackQueryUpdates
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
            try
            {
                _authorizer = BotInitializer.Instance.Authorizer;
                _botSettings = BotInitializer.Instance.BotSettings;
                _botActions = new BotActions(_botSettings.Token);
                _dbWorker = new GmailDbContextWorker();
                InitRules();
                BotInitializer.Instance.UpdatesHandler.TelegramCallbackQueryEvent += Handle;
            }
            catch (Exception ex)
            {
                throw new TypeInitializationException(nameof(CallbackQueryHandler), ex);
            }
        }

        public async void Handle(Query query)
        {
            if (query?.Data == null)
                throw new ArgumentNullException(nameof(query));

            CallbackData data;
            var callbackDataType = query.Data.Split(CallbackData.SEPARATOR).Last();
            if (int.Parse(callbackDataType) == (int)CallbackDataType.GetCallbackData)
                data = CallbackData.Create<GetCallbackData>(query.Data);
            else if (int.Parse(callbackDataType) == (int)CallbackDataType.SendCallbackData)
                data = CallbackData.Create<SendCallbackData>(query.Data);
            else
                return;

            foreach (var rule in _rules)
            {
                var rate = rule.Handle(data, this);
                if (rate == null) continue;
                LogMaker.Log(Logger, $"{query.Data} command received from user with id {(string)query.From}", false);
                Exception exception = null;
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
                catch (TelegramMethodsException ex)
                {
                    exception = ex;
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
                            $"An exception has been thrown in processing CallbackQuery with data: {query.Data}");
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
            _rules.Add(new ShowAttachmentsRule());
            _rules.Add(new HideAttachmentsRule());
            _rules.Add(new GetAttachmentRule());
            _rules.Add(new AddSubjectRule());
            _rules.Add(new AddTextMessageRule());
            _rules.Add(new SaveAsDraftRule());
            _rules.Add(new NotSaveAsDraftRule());
            _rules.Add(new CotinueWithOldRule());
            _rules.Add(new ContinueFromDraftRule());
            _rules.Add(new SendMessageRule());
            _rules.Add(new RemoveItemNewMessageRule());
        }


        private readonly List<ICallbackQueryHandlerRules> _rules = new List<ICallbackQueryHandlerRules>();
        private readonly BotActions _botActions;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GmailDbContextWorker _dbWorker;
        private readonly BotSettings _botSettings;
        private readonly Authorizer _authorizer;
    }

}