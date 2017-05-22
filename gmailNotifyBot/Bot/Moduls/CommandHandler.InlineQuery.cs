using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CoffeeJelly.gmailNotifyBot.Bot.Exceptions;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult;

namespace CoffeeJelly.gmailNotifyBot.Bot.Moduls
{
    public sealed partial class CommandHandler
    {
        private async void _updatesHandler_TelegramInlineQueryEvent(InlineQuery inlineQuery)
        {
            if (inlineQuery?.Query == null)
                throw new ArgumentNullException(nameof(inlineQuery));

            if (!inlineQuery.Query.StartsWithAny(StringComparison.CurrentCultureIgnoreCase,
                Commands.INBOX_INLINE_QUERY_COMMAND, Commands.ALL_INLINE_QUERY_COMMAND, Commands.RECIPIENTS_INLINE_QUERY_COMMAND)) return;

            LogMaker.Log(Logger, $"{inlineQuery.Query} command received from user with id {(string)inlineQuery.From}", false);
            try
            {
                var splittedQuery = inlineQuery.Query.Split(" ".ToCharArray(), 2);
                var queryArguments = splittedQuery.Length > 1 ? splittedQuery[1] : "";

                if (inlineQuery.Query.StartsWithAny(StringComparison.CurrentCultureIgnoreCase,
                    Commands.INBOX_INLINE_QUERY_COMMAND, Commands.ALL_INLINE_QUERY_COMMAND))
                {
                    var labelId = "";
                    if (inlineQuery.Query.StartsWith(Commands.INBOX_INLINE_QUERY_COMMAND))
                        labelId = "INBOX";

                    int page = 1;
                    if (queryArguments.StartsWith("p:"))
                    {
                        page = Int32.TryParse(queryArguments.Remove(0, 2), out page) == false ? 1 : page;
                        queryArguments = null;
                    }
                    await HandleShowMessagesInlineQueryCommand(inlineQuery, labelId, page, queryArguments);
                }
                else if (inlineQuery.Query.StartsWith(Commands.RECIPIENTS_INLINE_QUERY_COMMAND,
                    StringComparison.CurrentCultureIgnoreCase))
                {
                    await HandleShowContactsInlineQueryCommand(inlineQuery);
                }
            }
            catch (ServiceNotFoundException ex)
            {
                LogMaker.Log(Logger, ex);
                await _botActions.WrongCredentialsMessage(inlineQuery.From);
            }
            catch (Exception ex)
            {
                LogMaker.Log(Logger, ex, $"An exception has been thrown in processing InlineQuery with command {inlineQuery.Query}");
            }
        }

        private async void _updatesHandler_TelegramChosenInlineEvent(ChosenInlineResult chosenInlineResult)
        {
            if (chosenInlineResult?.Query == null)
                throw new ArgumentNullException(nameof(chosenInlineResult), $"{nameof(chosenInlineResult.Query)} must not be a null.");
            if (chosenInlineResult.ResultId == null)
                throw new ArgumentNullException(nameof(chosenInlineResult), $"{nameof(chosenInlineResult.ResultId)} must not be a null.");

            var logCommandReceived = new Action<string>(command => LogMaker.Log(Logger, $"{command} command received from user with id {(string)chosenInlineResult.From}", false));
            if (chosenInlineResult.Query.EqualsAny(Commands.INBOX_INLINE_QUERY_COMMAND, Commands.ALL_INLINE_QUERY_COMMAND,
                Commands.RECIPIENTS_INLINE_QUERY_COMMAND))
            {
                logCommandReceived(chosenInlineResult.Query);
                try
                {
                    if (chosenInlineResult.Query.EqualsAny(Commands.INBOX_INLINE_QUERY_COMMAND, Commands.ALL_INLINE_QUERY_COMMAND))
                        await HandleGetMesssagesChosenInlineResult(chosenInlineResult);
                    else if (chosenInlineResult.Query.Equals(Commands.RECIPIENTS_INLINE_QUERY_COMMAND))
                        await HandleShowContactsChosenInlineResult(chosenInlineResult);
                }
                catch (Exception ex)
                {
                    LogMaker.Log(Logger, ex);
                    Debug.Assert(false, $"Message to chat about _updatesHandler_TelegramChosenInlineEvent exeption");
                }
            }
        }

        private async Task HandleShowMessagesInlineQueryCommand(InlineQuery inlineQuery, string labelId, int page = 1, string searchExpression = null)
        {
            var resultsPerPage = 50;
            var messagesInOneResponse = 10;
            int offset;
            Int32.TryParse(inlineQuery.Offset, out offset);
            if (offset == -1)
                return;
            if (offset >= resultsPerPage)
            {
                page++;
                offset = offset - resultsPerPage;
            }
            var formatedMessages = await GetMessages(inlineQuery, offset, labelId, page, searchExpression, resultsPerPage, messagesInOneResponse);
            if (formatedMessages.Count == 0) return;
            if (formatedMessages.Count == messagesInOneResponse)
                await _botActions.ShowShortMessageAnswerInlineQuery(inlineQuery.Id, formatedMessages, offset + messagesInOneResponse);
            else
                await _botActions.ShowShortMessageAnswerInlineQuery(inlineQuery.Id, formatedMessages); //last response
        }

        private async Task HandleGetMesssagesChosenInlineResult(ChosenInlineResult sender)
        {
            var messageId = sender.ResultId;
            var formattedMessage = await GetMessage(sender.From, messageId);
            var isIgnored = await _dbWorker.IsPresentInIgnoreListAsync(sender.From, formattedMessage.From.Email);
            await _botActions.ShowShortMessageAsync(sender.From, formattedMessage, isIgnored);
        }

        private async Task HandleShowContactsInlineQueryCommand(InlineQuery inlineQuery, int page = 1, string searchExpression = null)
        {
            var resultsPerPage = 50;
            var messagesInOneResponse = 10;
            int offset;
            Int32.TryParse(inlineQuery.Offset, out offset);
            if (offset == -1)
                return;
            if (offset >= resultsPerPage)
            {
                page++;
                offset = offset - resultsPerPage;
            }
            var formatedMessages = await GetMessages(inlineQuery, offset, "SENT", page, searchExpression, resultsPerPage, messagesInOneResponse);
            var uniqueContacts = GetUniqueContactsFromMessageList(formatedMessages);
            if (uniqueContacts.Count == messagesInOneResponse)
                await _botActions.ShowContactsAnswerInlineQuery(inlineQuery.Id, uniqueContacts, offset + messagesInOneResponse);
            else
                await _botActions.ShowContactsAnswerInlineQuery(inlineQuery.Id, uniqueContacts); //last response
        }

        private async Task HandleShowContactsChosenInlineResult(ChosenInlineResult sender)
        {
            var formattedMessage = await GetMessage(sender.From, sender.ResultId);
            //await _botActions.UpdateNewMailMessage(sender.From, se);
        }
    }


}