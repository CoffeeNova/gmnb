
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage
{
    internal interface ISendKeyboardFactory
    {
        SendKeyboard CreateKeyboard(SendKeyboardState state, NmStoreModel model, string draftId);
    }

    internal class SendKeyboardFactory : ISendKeyboardFactory
    {
        public SendKeyboard CreateKeyboard(SendKeyboardState state, NmStoreModel model = null, string draftId = "")
        {
            SendKeyboard keyboard;
            switch (state)
            {
                case SendKeyboardState.Init:
                    keyboard = new InitKeyboard(null);
                    break;
                case SendKeyboardState.Store:
                    keyboard = new StoreKeyboard(model);
                    break;
                case SendKeyboardState.Continue:
                    keyboard = new InitKeyboard(model);
                    break;
                case SendKeyboardState.Drafted:
                    keyboard = new DraftedKeyboard(model, draftId);
                    break;
                default:
                    return null;
            }
            keyboard.CreateInlineKeyboard();
            return keyboard;
        }
    }

}
