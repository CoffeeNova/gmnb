using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.General
{
    internal abstract class GeneralKeyboard : Keyboard
    {
        public override void CreateInlineKeyboard()
        {
            GeneralCallbackData = new GeneralCallbackData
            {
                MessageKeyboardState = State
            };
            ButtonsInitializer();
            base.InlineKeyboard = DefineInlineKeyboard();
        }

        protected virtual InlineKeyboardButton InitButton(InlineKeyboardType type, string text, string command, bool forceCallbackData = true)
        {
            if (!forceCallbackData)
                return base.InitButton(type, text, command);
            var callbackData = new GeneralCallbackData(GeneralCallbackData)
            {
                Command = command
            };

            return base.InitButton(type, text, callbackData);
        }

        protected abstract GeneralKeyboardState State { get; }
        protected GeneralCallbackData GeneralCallbackData;
    }
}