using System.Runtime.Serialization;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage
{
    public enum SendKeyboardState
    {
        [EnumMember(Value = "init")]
        Init,
        [EnumMember(Value = "continue")]
        Continue,
        [EnumMember(Value = "store")]
        Store,
        [EnumMember(Value = "drafted")]
        Drafted,
        [EnumMember(Value = "sentSuccessful")]
        SentSuccessful,
        [EnumMember(Value = "sentWithError")]
        SentWithError
    }
}