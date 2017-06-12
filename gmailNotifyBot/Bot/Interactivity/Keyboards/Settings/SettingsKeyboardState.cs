using System.Runtime.Serialization;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    public enum SettingsKeyboardState
    {
        [EnumMember(Value = "mainMenu")]
        MainMenu,
        [EnumMember(Value = "labels")]
        Labels,
        [EnumMember(Value = "whiteList")]
        WhiteList,
        [EnumMember(Value = "blackList")]
        BlackList,
        [EnumMember(Value = "removeList")]
        RemoveList,
        [EnumMember(Value = "editList")]
        EditList,
        [EnumMember(Value = "ignore")]
        Ignore,
        [EnumMember(Value = "permissions")]
        Permissions
    }
}