
namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public interface IUserSettingModelRelation
    {
        int Id { get; set; }
        UserSettingsModel UserSettingsModel { get; set; }
        int UserSettingsModelId { get; set; }
    }
}