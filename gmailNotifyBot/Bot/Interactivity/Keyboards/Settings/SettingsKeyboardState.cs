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
        [EnumMember(Value = "editLabelsList")]
        EditLabelsList,
        [EnumMember(Value = "ignore")]
        Ignore,
        [EnumMember(Value = "permissions")]
        Permissions
    }

    public enum SelectedOption
    {
        None = 0,
        Option1 = 1,
        Option2 = 2,
        Option3 = 3,
        Option4 = 4,
        Option5 = 5,
        Option6 = 6,
        Option7 = 7,
        Option8 = 8,
        Option9 = 9,
        Option10 = 10,
    }
}