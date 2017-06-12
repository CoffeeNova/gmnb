using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal class NotifyKeyboard : GetKeyboard
    {
        internal NotifyKeyboard(FormattedMessage message) : base(message)
        {

        }

        protected override void ButtonsInitializer()
        {
            NotifyButton = IsIgnored
                ? InitButton(ActionButtonCaption.Unignore, CallbackCommand.UNIGNORE_COMMAND)
                : InitButton(ActionButtonCaption.Ignore, CallbackCommand.IGNORE_COMMAND);

            OpenWebButton = InitButton(ActionButtonCaption.OpenWeb, CallbackCommand.OPEN_WEB_COMMAND);
        }

        protected override GetKeyboardState State => GetKeyboardState.Minimized;

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            MainRow = new List<InlineKeyboardButton>();
            if (OpenWebButton != null)
                MainRow.Add(OpenWebButton);
            if (NotifyButton != null)
                MainRow.Add(NotifyButton);
            var inlineKeyboard = new List<List<InlineKeyboardButton>> { MainRow };
            return inlineKeyboard;
        }

        protected List<InlineKeyboardButton> MainRow;

        protected virtual InlineKeyboardButton NotifyButton { get; set; }

        protected virtual InlineKeyboardButton OpenWebButton { get; set; }

    }


}