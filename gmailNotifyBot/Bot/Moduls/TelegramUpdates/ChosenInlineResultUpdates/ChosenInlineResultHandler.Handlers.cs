using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage;
using CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult;
using Google.Apis.Gmail.v1;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ChosenInlineResultUpdates
{
    public partial class ChosenInlineResultHandler
    {
        public async Task HandleGetMesssagesChosenInlineResult(ChosenInlineResult sender)
        {
            var messageId = sender.ResultId;
            var formattedMessage = await Methods.GetMessage(sender.From, messageId);
            await _botActions.ShowShortMessageAsync(sender.From, formattedMessage);
        }

        public async Task HandleSetToChosenInlineResult(ChosenInlineResult sender)
        {
            await HandleRecipientChosenInlineResult<ToModel>(sender, "To");
        }

        public async Task HandleSetCcChosenInlineResult(ChosenInlineResult sender)
        {
            await HandleRecipientChosenInlineResult<CcModel>(sender, "Cc");
        }

        public async Task HandleSetBccChosenInlineResult(ChosenInlineResult sender)
        {
            await HandleRecipientChosenInlineResult<BccModel>(sender, "Bcc");
        }

        private async Task HandleRecipientChosenInlineResult<T>(ChosenInlineResult sender, string recipentProperyName) where T : class, IAddressModel, new()
        {
            var model = await _dbWorker.FindNmStoreAsync(sender.From);
            if (model == null)
            {
                await _botActions.SendLostInfoMessage(sender.From);
                return;
            }
            if (!Methods.EmailAddressValidation(sender.ResultId))
            {
                await _botActions.NotRecognizedEmailMessage(sender.From, sender.ResultId);
                return;
            }
            var property = model.GetPropertyValue(recipentProperyName) as ICollection<T>;
            var addressModel = new T {Address = sender.ResultId};
            property?.Add(addressModel);
            await _dbWorker.UpdateNmStoreRecordAsync(model);
            await _botActions.UpdateNewMailMessage(sender.From, SendKeyboardState.Continue, model);
        }

        public async Task HandleEditDraftChosenInlineResult(ChosenInlineResult sender)
        {
            var draftId = sender.ResultId;
            var nmStore = await _dbWorker.FindNmStoreAsync(sender.From);
            if (nmStore == null)
            {
                var draft = await Methods.GetDraft(sender.From, draftId,
                                    UsersResource.DraftsResource.GetRequest.FormatEnum.Full);
                nmStore = await _dbWorker.AddNewNmStoreAsync(sender.From);
                var formattedMessage = new FormattedMessage(draft);
                Methods.ComposeNmStateModel(nmStore, formattedMessage);
                var textMessage = await _botActions.SpecifyNewMailMessage(sender.From, SendKeyboardState.Continue, nmStore);
                nmStore.MessageId = textMessage.MessageId;
                nmStore.DraftId = draft.Id;
                await _dbWorker.UpdateNmStoreRecordAsync(nmStore);
            }
            else
                await _botActions.SaveAsDraftQuestionMessage(sender.From, SendKeyboardState.Store);
        }
    }
}