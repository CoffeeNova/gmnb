using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage
{
    internal abstract class SendKeyboard : Keyboard
    {
        protected SendKeyboard(NmStoreModel model)
        {
            Model = model;
        }

        public override void CreateInlineKeyboard()
        {
            GeneralCallbackData = new SendCallbackData
            {
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
                    Command = callbackCommand,
                    MessageId = Model?.MessageId
                }
            };
        }

        protected readonly NmStoreModel Model;
        protected SendCallbackData GeneralCallbackData;
        protected abstract SendKeyboardState State { get; }
    }
}