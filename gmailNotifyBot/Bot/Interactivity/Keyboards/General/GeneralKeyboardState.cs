using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.General
{
    public enum GeneralKeyboardState
    {
        [EnumMember(Value = "resumeNotifications")]
        ResumeNotifications,
        [EnumMember(Value = "reauthorize")]
        Reauthorize
    }
}