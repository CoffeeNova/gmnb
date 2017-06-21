using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.General
{
    internal class ResumeNotificationsKeyboard : GeneralKeyboard
    {
        protected override void ButtonsInitializer()
        {
            ResumeButton = InitButton(InlineKeyboardType.CallbackData, GeneralButtonCaption.ResumeNotifications, CallbackCommand.RESUME_NOTIFICATION_COMMAND);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            ResumeRow = new List<InlineKeyboardButton>();
            if (ResumeButton != null)
                ResumeRow.Add(ResumeButton);

            var inlineKeyboard = new List<List<InlineKeyboardButton>> { ResumeRow };
            return inlineKeyboard;
        }

        protected InlineKeyboardButton ResumeButton { get; set; }

        protected List<InlineKeyboardButton> ResumeRow;

        protected override GeneralKeyboardState State { get; } = GeneralKeyboardState.ResumeNotifications;
    }
}