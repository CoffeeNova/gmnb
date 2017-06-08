using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
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
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="forceCallbackData"></param>
        protected virtual InlineKeyboardButton InitButton(InlineKeyboardType type, string text, string command, string draftId = "", 
                                                        NmStoreUnit row = default(NmStoreUnit), int column = -1, bool forceCallbackData = true)
        {
            if (!forceCallbackData)
                return base.InitButton(type, text, command);
            var callbackData = new SendCallbackData(GeneralCallbackData)
            {
                Command = command,
                MessageId = Model?.MessageId.ToString(),
                DraftId = draftId,
                Row = row,
                Column = column
            };
            return base.InitButton(type, text, callbackData);
        }

        protected readonly NmStoreModel Model;
        protected SendCallbackData GeneralCallbackData;
        protected abstract SendKeyboardState State { get; }
    }
}