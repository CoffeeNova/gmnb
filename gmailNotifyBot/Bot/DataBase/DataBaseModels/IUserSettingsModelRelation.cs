
namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public interface IUserSettingsModelRelation : ICommonModelKey
    {
        UserSettingsModel UserSettingsModel { get; set; }
        int UserSettingsModelId { get; set; }
    }

}