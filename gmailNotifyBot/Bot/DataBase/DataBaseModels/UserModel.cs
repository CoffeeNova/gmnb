using System;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;
using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class UserModel
    {
        public UserModel(User user = null)
        {
            if (user == null) return;
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            IssuedTimeUtc = DateTime.UtcNow;
        }

        public UserModel()
        {

        }

        public static implicit operator User(UserModel obj)
        {
            return new User
            {
                Id = obj.Id,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Username = obj.Username
            };
        }

        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public string FirstName { get; set; }

        [JsonIgnore]
        public string LastName { get; set; }

        [JsonIgnore]
        public string Username { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        //[JsonIgnore]
        //public DateTime ReceivingTimeUtc { get; set; }

        [JsonProperty("IssuedUtc")]
        //[NotMapped]
        public DateTime IssuedTimeUtc { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }


    }

}