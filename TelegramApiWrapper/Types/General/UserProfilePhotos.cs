﻿using System.Collections.Generic;
using CoffeeJelly.TelegramApiWrapper.Converters;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.General
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
        [JsonConverter(typeof(ArrayToListConverter<List<PhotoSize>>))]
        [JsonProperty("photos", Required = Required.Always)]
        public List<List<PhotoSize>> Photos { get; set; }
    }
}