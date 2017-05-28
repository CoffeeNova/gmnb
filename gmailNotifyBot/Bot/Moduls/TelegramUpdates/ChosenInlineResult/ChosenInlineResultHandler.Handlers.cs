using System.Threading.Tasks;

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

        public async Task HandleShowContactsChosenInlineResult(QueryResult.ChosenInlineResult sender)
        {
            var formattedMessage = await Methods.GetMessage(sender.From, sender.ResultId);
            CreateDraft
            //await _botActions.UpdateNewMailMessage(sender.From, se);
        }
    }
}