using System;
using System.Collections.Generic;
using CoffeeJelly.TelegramApiWrapper.Converters;
using CoffeeJelly.TelegramApiWrapper.Extensions;
using Newtonsoft.Json;

namespace CoffeeJelly.TelegramApiWrapper.Types.General
{
    /// <summary>
    /// Represents a game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Title of the game.
        /// </summary>
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        /// <summary>
        /// Description of the game.
        /// </summary>
        [JsonProperty("description", Required = Required.Always)]
        public string Description { get; set; }

        /// <summary>
        /// Photo that will be displayed in the game message in chats.
        /// </summary>
        [JsonConverter(typeof(ArrayToListConverter<PhotoSize>))]
        [JsonProperty("photo", Required = Required.Always)]
        public List<PhotoSize> Photo { get; set; }

        private string _text;

        /// <summary>
        /// Brief description of the game or high scores included in the game message. 
        /// Can be automatically edited to include current high scores for the game when the bot calls setGameScore, 
        /// or manually edited using editMessageText. 0-4096 characters.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("text")]
        public string Text
        {
            get { return _text; }
            set
            {
                if (value != null && !value.Length.InRange(0, 4096))
                    throw new ArgumentOutOfRangeException(nameof(value), $"{value} Length should be from 0 to 4096 characters.");
                _text = value;
            }
        }

        /// <summary>
        /// Special entities that appear in text, such as usernames, URLs, bot commands, etc.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonConverter(typeof(ArrayToListConverter<MessageEntity>))]
        [JsonProperty("text_entities")]
        public List<MessageEntity> TextEntities { get; set; }

        /// <summary>
        /// Animation that will be displayed in the game message in chats.
        /// </summary>
        /// <remarks>Optional.</remarks>
        [JsonProperty("animation")]
        public Animation Animation { get; set; }
    }
}