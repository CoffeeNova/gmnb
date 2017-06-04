using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ChosenInlineResultUpdates
{
    using QueryResult = TelegramBotApiWrapper.Types.InlineQueryResult;

    public partial class ChosenInlineResultHandler
    {
        public async Task HandleGetMesssagesChosenInlineResult(QueryResult.ChosenInlineResult sender)
        {
            var messageId = sender.ResultId;
            var formattedMessage = await Methods.GetMessage(sender.From, messageId);
            await _botActions.ShowShortMessageAsync(sender.From, formattedMessage);
        }

        public async Task HandleSetToChosenInlineResult(QueryResult.ChosenInlineResult sender)
        {
            await HandleRecipientChosenInlineResult(sender, "To");
        }

        public async Task HandleSetCcChosenInlineResult(QueryResult.ChosenInlineResult sender)
        {
            await HandleRecipientChosenInlineResult(sender, "Cc");
        }

        public async Task HandleSetBccChosenInlineResult(QueryResult.ChosenInlineResult sender)
        {
            await HandleRecipientChosenInlineResult(sender, "Bcc");
        }

        private async Task HandleRecipientChosenInlineResult(QueryResult.ChosenInlineResult sender, string recipentProperyName)
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
            var property = model.GetPropertyValue(recipentProperyName) as List<string>;
            property?.Add(sender.ResultId);
            await _botActions.UpdateNewMailMessage(sender.From, SendKeyboardState.Continue, model);
        }

    }
}