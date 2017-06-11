
namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal interface IGetKeyboardFactory
    {
        GetKeyboard CreateKeyboard(GetKeyboardState state, FormattedMessage message, int page, bool isIgnored);
    }

    internal class GetKeyboardFactory : IGetKeyboardFactory
    {
        public GetKeyboard CreateKeyboard(GetKeyboardState state, FormattedMessage message, int page=0, bool isIgnored=false)
        {
            GetKeyboard keyboard;
            switch (state)
            {
                case GetKeyboardState.Minimized:
                    keyboard = new MinimizedKeyboard(message);
                    break;
                case GetKeyboardState.Maximized:
                    keyboard = new MaximizedKeyboard(message);
                    break;
                case GetKeyboardState.MinimizedActions:
                    keyboard = new MinimizedActionsKeyboard(message);
                    break;
                case GetKeyboardState.MaximizedActions:
                    keyboard = new MaximizedActionsKeyboard(message);
                    break;
                case GetKeyboardState.Attachments:
                    keyboard = new AttachmentsKeyboard(message);
                    break;
                case GetKeyboardState.Notify:
                    keyboard = new NotifyKeyboard(message);
                    break;  
                default:
                    return null;
            }
            keyboard.Page = page;
            keyboard.IsIgnored = isIgnored;
            keyboard.CreateInlineKeyboard();
            return keyboard;
        }
    }

}
