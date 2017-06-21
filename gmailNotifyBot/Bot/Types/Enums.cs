namespace CoffeeJelly.gmailNotifyBot.Bot.Types
{
    public enum ModifyLabelsAction
    {
        Add,
        Remove
    }

    public enum Recipients
    {
        To,
        Cc,
        Bcc
    }

    public enum NmStoreUnit
    {
        None = 0,
        To = 1,
        Cc = 2,
        Bcc = 3,
        File = 4
    }

    public enum CallbackDataType
    {
        GetCallbackData,
        SendCallbackData,
        SettingsCallbackData,
        GeneralCallbackData
    }
}