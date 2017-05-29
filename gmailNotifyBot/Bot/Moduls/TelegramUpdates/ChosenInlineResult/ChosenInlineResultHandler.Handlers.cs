using System.Collections.Generic;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls.TelegramUpdates.ChosenInlineResult
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

        public async Task HandleSetToChosenInlineResult(QueryResult.ChosenInlineResult sender,
            SendCallbackData callbackData)
        {
            //var formattedMessage = await Methods.GetMessage(sender.From, sender.ResultId);
            Google.Apis.Gmail.v1.Data.Message draft = null;
            if (string.IsNullOrEmpty(callbackData.DraftId))
                draft = new Google.Apis.Gmail.v1.Data.Message();
            else
                draft = await Methods.GetDraft(sender.From, callbackData.DraftId);

            Methods.AddToDraftBody(draft, new List<string> {sender.ResultId});

            sender.Query
                await 
            _botActions.UpdateNewMailMessage(sender.From, se);
        }

        public async Task HandleSetCcChosenInlineResult(QueryResult.ChosenInlineResult sender,
            SendCallbackData callbackData)
        {
        }

        public async Task HandleSetBccChosenInlineResult(QueryResult.ChosenInlineResult sender,
            SendCallbackData callbackData)
        {
        }
    }
}