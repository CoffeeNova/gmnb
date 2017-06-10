
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;

namespace CoffeeJelly.gmailNotifyBot.Bot.Types
{
    internal class UserInfo : IUserInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }
}