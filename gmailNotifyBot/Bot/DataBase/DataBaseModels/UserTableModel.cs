using System;
using System.Data.Entity;
using CoffeeJelly.gmailNotifyBot.Bot.Telegram;
using Newtonsoft.Json;

namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public class UserModel
    {
        public UserModel(Chat chat = null)
        {
            if (chat == null) return;
            UserId = chat.Id;
            FirstName = chat.FirstName;
            LastName = chat.LastName;
            Username = chat.Username;
            ReceivingTime = DateTime.Now;
        }

        public UserModel()
        {
            
        }

        [JsonIgnore]
        public long Id { get; set; }

        [JsonIgnore]
        public long UserId { get; set; }

        [JsonIgnore]
        public string FirstName { get; set; }

        [JsonIgnore]
        public string LastName { get; set; }

        [JsonIgnore]
        public string Username { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonIgnore]
        public DateTime ReceivingTime { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }


    }

    public class PendingUserModel
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string State { get; set; }

        public DateTime JoinTime { get; set; }
    }

    public class UserContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }

        public DbSet<PendingUserModel> PendingUser { get; set; }

    }

}