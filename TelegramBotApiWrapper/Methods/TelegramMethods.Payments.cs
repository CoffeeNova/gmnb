using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Helpers;
using CoffeeJelly.TelegramBotApiWrapper.JsonParsers;
using CoffeeJelly.TelegramBotApiWrapper.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using CoffeeJelly.TelegramBotApiWrapper.Types.Messages;
using CoffeeJelly.TelegramBotApiWrapper.Types.Payments;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Methods
{
    public partial class TelegramMethods
    {
        /// <summary>
        /// Use this method to send invoices.
        /// </summary>
        /// <param name="chatId">Unique identifier for the target private chat.</param>
        /// <param name="title">Product name.</param>
        /// <param name="description">Product description.</param>
        /// <param name="payload">Bot-defined invoice payload, 1-128 bytes. This will not be displayed to the user, use for your internal processes.</param>
        /// <param name="providerToken">Payments provider token, obtained via Botfather.</param>
        /// <param name="startParameter">Unique deep-linking parameter that can be used to generate this invoice when used as a start parameter.</param>
        /// <param name="currency">Three-letter ISO 4217 currency code, see more on <see href="https://core.telegram.org/bots/payments#supported-currencies">currencies</see>/>.</param>
        /// <param name="prices">Price breakdown, a list of components (e.g. product price, tax, discount, delivery cost, delivery tax, bonus, etc.).</param>
        /// <param name="photoUrl">URL of the product photo for the invoice. Can be a photo of the goods or a marketing image for a service. People like it better when they see what they are paying for.</param>
        /// <param name="photoSize">Photo size.</param>
        /// <param name="photoWidth">Photo width.</param>
        /// <param name="photoHeight">Photo height.</param>
        /// <param name="needName">Pass <see langword="true"/>, if you require the user's full name to complete the order.</param>
        /// <param name="needPhoneNumber">Pass <see langword="true"/>, if you require the user's phone number to complete the order.</param>
        /// <param name="needEmail">Pass <see langword="true"/>, if you require the user's email to complete the order.</param>
        /// <param name="needShipingAddress">Pass <see langword="true"/>, if you require the user's shipping address to complete the order.</param>
        /// <param name="isFlexible">Pass <see langword="true"/>, if the final price depends on the shipping method.</param>
        /// <param name="disableNotification">Sends the message silently. Users will receive a notification with no sound.</param>
        /// <param name="replyToMessageId">If the message is a reply, ID of the original message</param>
        /// <param name="replyMarkup">A JSON-serialized object for an inline keyboard. If empty, one 'Pay total price' button will be shown. If not empty, the first button must be a Pay button.</param>
        /// <returns><see cref="InvoiceMessage"/></returns>
        public async Task<InvoiceMessage> SendInvoice(string chatId, string title, string description, string payload, string providerToken,
            string startParameter, string currency, List<LabeledPrice> prices, string photoUrl = null, int photoSize = 0,
            int photoWidth = 0, int photoHeight = 0, bool needName = false, bool needPhoneNumber = false, bool needEmail = false,
            bool needShipingAddress = false, bool isFlexible = false,
            bool disableNotification = false, int? replyToMessageId = null, IMarkup replyMarkup = null)
        {
            chatId.NullInspect(nameof(chatId));
            title.NullInspect(nameof(title));
            description.NullInspect(nameof(description));
            payload.NullInspect(nameof(payload));
            providerToken.NullInspect(nameof(providerToken));
            startParameter.NullInspect(nameof(startParameter));
            currency.NullInspect(nameof(currency));
            prices.NullInspect(nameof(prices));

            var content = new Content {Json = true};
            content.Add("title", title);
            content.Add("description", description);
            content.Add("payload", payload);
            content.Add("provider_token", providerToken);
            content.Add("start_parameter", startParameter);
            content.Add("currency", chatId);
            content.Add("chat_id", currency);
            content.Add("prices", JsonConvert.SerializeObject(prices, Formatting.None, Settings));

            if (photoUrl != null)
                content.Add("photo_url", photoUrl);
            if (photoSize != 0)
                content.Add("photo_size", photoSize.ToString());
            if (photoWidth != 0)
                content.Add("photo_width", photoWidth.ToString());
            if (photoHeight != 0)
                content.Add("photo_height", photoHeight.ToString());
            if (needName)
                content.Add("need_name", true.ToString());
            if (needPhoneNumber)
                content.Add("need_phone_number", true.ToString());
            if (needEmail)
                content.Add("need_email", true.ToString());
            if (needShipingAddress)
                content.Add("need_shiping_address", true.ToString());
            if (isFlexible)
                content.Add("is_flexible", true.ToString());

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<InvoiceMessage>(httpContent).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// If you sent an invoice requesting a shipping address and the parameter <see langword="isFlexible"/> was specified, 
        /// the Bot API will send an <see cref="Update"/>  with a <see langword="shipping_query"/> field to the bot. Use this method to reply to shipping queries. 
        /// On success, <see langword="true"/> is returned.
        /// </summary>
        /// <param name="shippingQueryId">Unique identifier for the query to be answered.</param>
        /// <param name="ok">Specify <see langword="true"/> if delivery to the specified address is possible and <see langword="false"/> 
        /// if there are any problems (for example, if delivery to the specified address is not possible)</param>
        /// <param name="shippingOptions">Required if <paramref name="ok"/> is <see langword="true"/>. A JSON-serialized array of available shipping options.</param>
        /// <param name="errorMessage">Required if <paramref name="ok"/> is <see langword="false"/>. 
        /// Error message in human readable form that explains why it is impossible to complete the order (e.g. "Sorry, delivery to your desired address is unavailable'). 
        /// Telegram will display this message to the user.</param>
        /// <returns><see langword="true"/> on success.</returns>
        public async Task<bool> AnswerShippingQuery(string shippingQueryId, bool ok, List<ShippingOption> shippingOptions = null,
            string errorMessage = null)
        {
            shippingQueryId.NullInspect(nameof(shippingQueryId));
            if (ok && shippingOptions == null)
                throw new ArgumentException($"Argument must be not null if {nameof(ok)} is true.", nameof(shippingOptions));
            if (!ok && errorMessage == null)
                throw new ArgumentException($"Argument must be not null if {nameof(ok)} is true.", nameof(errorMessage));

            var content = new Content {Json = true};
            content.Add("shipping_query_id", shippingQueryId);
            content.Add("ok", ok.ToString());
            if (shippingOptions != null)
                content.Add("shipping_options", JsonConvert.SerializeObject(shippingOptions, Formatting.None, Settings));
            if (errorMessage != null)
                content.Add("error_message", errorMessage);

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<bool>(httpContent).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Once the user has confirmed their payment and shipping details, the Bot API sends the final confirmation in the form of an <see cref="Update"/> 
        /// with the <see langword="pre_checkout_query"/> field . 
        /// Use this method to respond to such pre-checkout queries.
        /// </summary>
        /// <param name="preCheckoutQueryId"></param>
        /// <param name="ok"></param>
        /// <param name="errorMessage"></param>
        /// <returns>On success, <see langword="true"/> is returned.</returns>
        /// <remarks>The Bot API must receive an answer within 10 seconds after the pre-checkout query was sent.</remarks>
        public async Task<bool> AnswerPreCheckoutQuery(string preCheckoutQueryId, bool ok, string errorMessage = null)
        {
            preCheckoutQueryId.NullInspect(nameof(preCheckoutQueryId));
            if (!ok && errorMessage == null)
                throw new ArgumentException($"Argument must be not null if {nameof(ok)} is true.", nameof(errorMessage));

            var content = new Content { Json = true };
            content.Add("pre_checkout_query_id", preCheckoutQueryId);
            content.Add("ok", ok.ToString());
            if (errorMessage != null)
                content.Add("error_message", errorMessage);

            var json = JsonConvert.SerializeObject(content.JsonData);
            using (var httpContent = new StringContent(json, Encoding.UTF8, "application/json"))
            {
                return await UploadFormData<bool>(httpContent).ConfigureAwait(false);
            }
        }
    }
}
