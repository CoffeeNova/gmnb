using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
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
                InitFullAccessRules();
                InitNotifyAccessRules();
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
            else if (int.Parse(callbackDataType) == (int)CallbackDataType.SettingsCallbackData)
                data = CallbackData.Create<SettingsCallbackData>(query.Data);
            else
                return;

            Exception exception = null;
            try
            {
                var resultInvoke = await RuleInvokeAction(_authRule, data, query);
                if (resultInvoke) return;

                var service = Methods.SearchServiceByUserId(query.From);
                var rules = service.FullUserAccess
                    ? _fullAccessRules
                    : _notifyAccessRules;
                foreach (var rule in rules)
                    await RuleInvokeAction(rule, data, query, service);
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

        private async Task<bool> RuleInvokeAction(ICallbackQueryHandlerRule rule, CallbackData data, Query query, Service service = null)
        {
            var rate = rule.Handle(data, service, this);
            if (rate == null)
                return false;
            LogMaker.Log(Logger, $"{query.Data} command received from user with id {(string)query.From}", false);
            await rate.Invoke(query);
            return true;
        }

        private void InitNotifyAccessRules()
        {
            _notifyAccessRules.Add(new UnignoreRule());
            _notifyAccessRules.Add(new IgnoreRule());

            #region settings

            _notifyAccessRules.Add(new OpenLabelsMenuRule());
            _notifyAccessRules.Add(new OpenPermissionsMenuRule());
            _notifyAccessRules.Add(new OpenIgnoreListMenuRule());
            _notifyAccessRules.Add(new StartNotifyRule());
            _notifyAccessRules.Add(new StopNotifyRule());
            _notifyAccessRules.Add(new ShowAboutRule());
            _notifyAccessRules.Add(new OpenWhitelistMenuRule());
            _notifyAccessRules.Add(new OpenBlacklistMenuRule());
            _notifyAccessRules.Add(new BackToLabelsMenuRule());
            _notifyAccessRules.Add(new WhitelistLabelActionRule());
            _notifyAccessRules.Add(new BlacklistLabelActionRule());
            _notifyAccessRules.Add(new UseBlackListRule());
            _notifyAccessRules.Add(new UseWhiteListRule());
            _notifyAccessRules.Add(new DisplayIgnoredEmailRule());
            _notifyAccessRules.Add(new AddToIgnoreRule());
            _notifyAccessRules.Add(new RemoveFromIgnoreRule());
            _notifyAccessRules.Add(new SwapPermissionsRule());
            _notifyAccessRules.Add(new RevokePermissionsRule());
            _notifyAccessRules.Add(new RevokePermissionsViaWebRule());
            _notifyAccessRules.Add(new BackToMainMenuRule());

            #endregion
        }

        private void InitFullAccessRules()
        {
            _fullAccessRules.Add(new ExpandRule());
            _fullAccessRules.Add(new HideRule());
            _fullAccessRules.Add(new ExpandActionsRule());
            _fullAccessRules.Add(new HideActionsRule());
            _fullAccessRules.Add(new ToReadRule());
            _fullAccessRules.Add(new ToUnReadRule());
            _fullAccessRules.Add(new ToSpamRule());
            _fullAccessRules.Add(new ToInboxRule());
            _fullAccessRules.Add(new ToTrashRule());
            _fullAccessRules.Add(new ArchiveRule());
            _fullAccessRules.Add(new UnignoreRule());
            _fullAccessRules.Add(new IgnoreRule());
            _fullAccessRules.Add(new NextPageRule());
            _fullAccessRules.Add(new PrevPageRule());
            _fullAccessRules.Add(new ShowAttachmentsRule());
            _fullAccessRules.Add(new HideAttachmentsRule());
            _fullAccessRules.Add(new GetAttachmentRule());
            _fullAccessRules.Add(new AddSubjectRule());
            _fullAccessRules.Add(new AddTextMessageRule());
            _fullAccessRules.Add(new SaveAsDraftRule());
            _fullAccessRules.Add(new NotSaveAsDraftRule());
            _fullAccessRules.Add(new CotinueWithOldRule());
            _fullAccessRules.Add(new ContinueFromDraftRule());
            _fullAccessRules.Add(new SendMessageRule());
            _fullAccessRules.Add(new RemoveItemNewMessageRule());

            #region Settings

            _fullAccessRules.Add(new OpenLabelsMenuRule());
            _fullAccessRules.Add(new OpenPermissionsMenuRule());
            _fullAccessRules.Add(new OpenIgnoreListMenuRule());
            _fullAccessRules.Add(new StartNotifyRule());
            _fullAccessRules.Add(new StopNotifyRule());
            _fullAccessRules.Add(new ShowAboutRule());
            _fullAccessRules.Add(new CreateNewLabelRule());
            _fullAccessRules.Add(new OpenEditLabelsMenuRule());
            _fullAccessRules.Add(new OpenWhitelistMenuRule());
            _fullAccessRules.Add(new OpenBlacklistMenuRule());
            _fullAccessRules.Add(new BackToLabelsMenuRule());
            _fullAccessRules.Add(new OpenLabelActionsMenuRule());
            _fullAccessRules.Add(new WhitelistLabelActionRule());
            _fullAccessRules.Add(new BlacklistLabelActionRule());
            _fullAccessRules.Add(new UseBlackListRule());
            _fullAccessRules.Add(new UseWhiteListRule());
            _fullAccessRules.Add(new EditLabelNameRule());
            _fullAccessRules.Add(new RemoveLabelRule());
            _fullAccessRules.Add(new BackToEditLabelsListMenuRule());
            _fullAccessRules.Add(new DisplayIgnoredEmailRule());
            _fullAccessRules.Add(new AddToIgnoreRule());
            _fullAccessRules.Add(new RemoveFromIgnoreRule());
            _fullAccessRules.Add(new SwapPermissionsRule());
            _fullAccessRules.Add(new RevokePermissionsRule());
            _fullAccessRules.Add(new RevokePermissionsViaWebRule());
            _fullAccessRules.Add(new BackToMainMenuRule());

            #endregion
        }

        private readonly ICallbackQueryHandlerRule _authRule = new SendAuthorizeLinkRule();
        private readonly List<ICallbackQueryHandlerRule> _fullAccessRules = new List<ICallbackQueryHandlerRule>();
        private readonly List<ICallbackQueryHandlerRule> _notifyAccessRules = new List<ICallbackQueryHandlerRule>();
        private readonly BotActions _botActions;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly GmailDbContextWorker _dbWorker;
        private readonly BotSettings _botSettings;
        private readonly Authorizer _authorizer;
    }

}