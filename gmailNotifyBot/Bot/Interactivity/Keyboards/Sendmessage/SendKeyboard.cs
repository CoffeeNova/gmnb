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
        /// <param name="type"></param>
        /// <param name="text"></param>
        /// <param name="command"></param>
        /// <param name="draftId"></param>
        /// <param name="isCallbackData"></param>
        protected virtual InlineKeyboardButton InitButton(InlineKeyboardType type, string text, string command, string draftId = "", bool isCallbackData = true)
        {
            if (!isCallbackData)
                return base.InitButton(type, text, command);
            var callbackData = new SendCallbackData(GeneralCallbackData)
            {
                Command = command,
                MessageId = Model?.MessageId.ToString(),
                DraftId = draftId
            };
            return base.InitButton(type, text, callbackData);
        }

        protected readonly NmStoreModel Model;
        protected SendCallbackData GeneralCallbackData;
        protected abstract SendKeyboardState State { get; }
    }
}