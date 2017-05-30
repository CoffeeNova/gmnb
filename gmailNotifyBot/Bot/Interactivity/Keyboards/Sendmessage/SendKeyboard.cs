using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage
{
    internal abstract class SendKeyboard : Keyboard
    {
        protected SendKeyboard(FormattedMessage draft)
        {
            Draft = draft;
        }

        public override void CreateInlineKeyboard()
        {
            GeneralCallbackData = new SendCallbackData
            {
                DraftId = Draft?.Id,
                MessageKeyboardState = State
            };
            ButtonsInitializer();
            base.InlineKeyboard = DefineInlineKeyboard();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="callbackCommand"></param>
        /// <returns>Do not use virtual members from constructor!</returns>
        protected virtual InlineKeyboardButton InitButton(string text, string callbackCommand)
        {
            return new InlineKeyboardButton
            {
                Text = text,
                CallbackData = new SendCallbackData(GeneralCallbackData)
                {
                    Command = callbackCommand
                }
            };
        }

        protected readonly FormattedMessage Draft;
        protected SendCallbackData GeneralCallbackData;
        protected abstract SendKeyboardState State { get; }
    }
}