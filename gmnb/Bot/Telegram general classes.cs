using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeJelly.gmnb.Bot
{
    /// <summary>
    /// This class represents one special entity in a text message. For example, hashtags, usernames, URLs, etc.
    /// </summary>
    public class MessageEntity
    {
        /// <summary>
        /// Type of the entity.
        /// </summary>
        /// <remarks>
        /// Can be mention (@username), hashtag, bot_command, url, email, bold (bold text), italic (italic text), 
        /// code (monowidth string), pre (monowidth block), text_link (for clickable text URLs), 
        /// text_mention (for users without usernames)
        /// </remarks>
        public string Type { get; set; }

        /// <summary>
        /// Offset in UTF-16 code units to the start of the entity.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Length of the entity in UTF-16 code units.
        /// </summary>
        public int Lenght { get; set; }

        /// <summary>
        /// For “text_link” only, url that will be opened after user taps on the text.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// For “text_mention” only, the mentioned user.
        /// </summary>
        public User User { get; set; }
    }
}
