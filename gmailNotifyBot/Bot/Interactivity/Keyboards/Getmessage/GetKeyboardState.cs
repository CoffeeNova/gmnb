using System.Runtime.Serialization;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    public enum GetKeyboardState
    {
        [EnumMember(Value = "minimized")]
        Minimized,
        [EnumMember(Value = "maximized")]
        Maximized,
        [EnumMember(Value = "minimizedActions")]
        MinimizedActions,
        [EnumMember(Value = "maximizedActions")]
        MaximizedActions,
        [EnumMember(Value = "attachments")]
        Attachments
    }
}