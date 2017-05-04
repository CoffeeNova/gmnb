using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using CoffeeJelly.TelegramApiWrapper.Attributes;
using CoffeeJelly.TelegramApiWrapper.Extensions;
using CoffeeJelly.TelegramApiWrapper.Types.InlineQueryResult;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        /// <summary>
        /// Use this method to send answers to an inline query.
        /// </summary>
        /// <param name="inlineQueryId">Unique identifier for the answered query.</param>
        /// <param name="results">A JSON-serialized array of results for the inline query.</param>
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
        [TelegramMethod("answerInlineQuery")]
        public bool AnswerInlineQuery(string inlineQueryId, List<InlineQueryResult> results, int? cacheTime = null, bool? isPersonal = null,
                                       string nextOffset = null, string switchPmText = null, string switchPmParameter = null)
        {
            if (switchPmParameter != null)
                if (!Tools.OnlyAllowedCharacters1(switchPmParameter))
                    throw new ArgumentException("Forbidden charachters in argument. Only A-Z, a-z, 0-9, _ and - are allowed", nameof(switchPmParameter));
            inlineQueryId.NullInspect(nameof(inlineQueryId));
            results.NullInspect(nameof(results));
            if (results.Count > 50)
                throw new ArgumentException("No more than 50 results are allowed", nameof(results));

            var parameters = new NameValueCollection
            {
                {"inline_query_id", inlineQueryId},
                {"results", JsonConvert.SerializeObject(results, Formatting.None, Settings)}
            };
            if (cacheTime != null)
                parameters.Add("cache_time", cacheTime.ToString());
            if (isPersonal != null)
                parameters.Add("is_personal", isPersonal.ToString());
            if (nextOffset != null)
                parameters.Add("next_offset", nextOffset);
            if (switchPmText != null)
                parameters.Add("switch_pm_text", switchPmText);
            if (switchPmParameter != null)
                parameters.Add("switch_pm_parameter", switchPmParameter);

            var json = UploadUrlQuery(parameters);
            var result = (string)json["result"];
            return result == null ? false : Convert.ToBoolean(result);
        }
    }
}