
namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public interface INmStoreModelRelation
    {
        int Id { get; set; }
        NmStoreModel NmStoreModel { get; set; }
        int NmStoreModelId { get; set; }
    }
}