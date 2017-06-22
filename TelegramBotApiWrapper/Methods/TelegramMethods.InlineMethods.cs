using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading;
using CoffeeJelly.TelegramBotApiWrapper.Attributes;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.InlineQueryResult;
using Newtonsoft.Json;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Exceptions;
using CoffeeJelly.TelegramBotApiWrapper.Helpers;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        /// <summary>
        /// Use this method to send answers to an inline query.
        /// </summary>
        /// <param name="inlineQueryId">Unique identifier for the answered query.</param>
        /// <param name="results">A List of results containing <see cref="InlineQueryResult"/> types.</param>
        /// <param name="cacheTime">The maximum amount of time in seconds that the result of the inline query may be cached on the server. Defaults to 300.</param>
        /// <param name="isPersonal">
        /// Pass <see langword="true"/>, if results may be cached on the server side only for the user that sent the query. 
        /// By default, results may be returned to any user who sends the same query.
        /// </param>
        /// <param name="nextOffset">
        /// Pass the offset that a client should send in the next query with the same text to receive more results. 
        /// Pass an empty string if there are no more results or if you don‘t support pagination. Offset length can’t exceed 64 bytes.
        /// </param>
        /// <param name="switchPmText">
        /// If passed, clients will display a button with specified text that switches the user to a private chat with the bot 
        /// and sends the bot a start message with the parameter <paramref name="switchPmParameter"/>.
        /// </param>
        /// <param name="switchPmParameter">
        /// Deep-linking parameter for the /start message sent to the bot when user presses the switch button. 1-64 characters, only A-Z, a-z, 0-9, _ and - are allowed.
        /// </param>
        /// <returns>On success, <see langword="true"/> is returned.</returns>
        /// <remarks>No more than 50 results per query are allowed.</remarks>
        /// <exception cref="ArgumentException">Throws when <paramref name="switchPmParameter"/> has not allowed characters.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws when <paramref name="results"/> has more then 50 elements.</exception>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="inlineQueryId"/> or <paramref name="results"/> equals <see langword="null"/>.</exception>
        /// <exception cref="TelegramMethodsException">Throws when there is an error while using this method.</exception>
        [TelegramMethod("answerInlineQuery")]
        public async Task<bool> AnswerInlineQuery(string inlineQueryId, List<InlineQueryResult> results, int? cacheTime = null, bool? isPersonal = null,
                                       string nextOffset = null, string switchPmText = null, string switchPmParameter = null)
        {
            if (switchPmParameter != null)
                if (!Tools.OnlyAllowedCharacters1(switchPmParameter))
                    throw new ArgumentException("Forbidden charachters in argument. Only A-Z, a-z, 0-9, _ and - are allowed", nameof(switchPmParameter));
            inlineQueryId.NullInspect(nameof(inlineQueryId));
            results.NullInspect(nameof(results));
            if (results.Count > 50)
                throw new ArgumentOutOfRangeException(nameof(results), "No more than 50 results are allowed");

            var content = new Content { Json = true };
            content.Add("inline_query_id", inlineQueryId);
            content.Add("results", JsonConvert.SerializeObject(results, Formatting.None, Settings));

            if (cacheTime != null)
                content.Add("cache_time", cacheTime.ToString());
            if (isPersonal != null)
                content.Add("is_personal", isPersonal.ToString());
            if (nextOffset != null)
                content.Add("next_offset", nextOffset);
            if (switchPmText != null)
                content.Add("switch_pm_text", switchPmText);
            if (switchPmParameter != null)
                content.Add("switch_pm_parameter", switchPmParameter);

            var payload = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(payload, System.Text.Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<bool>(httpContent).ConfigureAwait(false);
            }
        }
    }

}