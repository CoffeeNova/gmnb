using System.Runtime.Serialization;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    public enum SettingsKeyboardState
    {
        [EnumMember(Value = "mainMenu")]
        MainMenu,
        [EnumMember(Value = "labelsMenu")]
        LabelsMenu,
        [EnumMember(Value = "whiteListMenu")]
        WhiteListMenu,
        [EnumMember(Value = "blackListMenu")]
        BlackListMenu,
        [EnumMember(Value = "editLabelsMenu")]
        EditLabelsMenu,
        [EnumMember(Value = "ignoreMenu")]
        IgnoreMenu,
        [EnumMember(Value = "permissionsMenu")]
        PermissionsMenu,
        [EnumMember(Value = "labelActionsMenu")]
        LabelActionsMenu
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