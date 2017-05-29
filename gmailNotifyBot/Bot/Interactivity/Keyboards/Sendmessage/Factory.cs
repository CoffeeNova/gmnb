
namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage
{
    internal interface ISendKeyboardFactory
    {
        SendKeyboard CreateKeyboard(SendKeyboardState state, FormattedMessage message);
    }

    internal class SendKeyboardFactory : ISendKeyboardFactory
    {
        public SendKeyboard CreateKeyboard(SendKeyboardState state, FormattedMessage draft = null)
        {
            SendKeyboard keyboard;
            switch (state)
            {
                case SendKeyboardState.Init:
                    keyboard = new InitKeyboard(draft);
                    break;
                default:
                    return null;
            }
            keyboard.CreateInlineKeyboard();
            return keyboard;
        }
    }

}
