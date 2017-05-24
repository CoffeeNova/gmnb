using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Converters;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramBotApiWrapper.Types.General
{
    /// <summary>
    /// Represent a user's profile pictures.
    /// </summary>
    public class UserProfilePhotos
    {
        /// <summary>
        /// Total number of profile pictures the target user has.
        /// </summary>
        [JsonProperty("total_count", Required = Required.Always)]
        public int TotalCount { get; set; }

        /// <summary>
        /// Requested profile pictures (in up to 4 sizes each).
        /// </summary>
        [JsonConverter(typeof(ArrayToIEnumerableConverter<List<PhotoSize>>))]
        [JsonProperty("photos", Required = Required.Always)]
        public IEnumerable<IEnumerable<PhotoSize>> Photos { get; set; }
    }
}