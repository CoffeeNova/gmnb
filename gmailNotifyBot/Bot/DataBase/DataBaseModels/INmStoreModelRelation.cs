
namespace CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels
{
    public interface INmStoreModelRelation : ICommonModelKey
    {
        NmStoreModel NmStoreModel { get; set; }
        int NmStoreModelId { get; set; }
    }

}