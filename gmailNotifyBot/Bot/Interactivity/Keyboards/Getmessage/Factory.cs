using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal interface IKeyboardFactory
    {
        Keyboard CreateKeyboard(MessageKeyboardState state, FormattedMessage message, int page, bool isIgnored);
    }

    internal class KeyboardFactory : IKeyboardFactory
    {
        public Keyboard CreateKeyboard(MessageKeyboardState state, FormattedMessage message, int page=0, bool isIgnored=false)
        {
            Keyboard keyboard;
            switch (state)
            {
                case MessageKeyboardState.Minimized:
                    keyboard = new MinimizedKeyboard(message);
                    break;
                case MessageKeyboardState.Maximized:
                    keyboard = new MaximizedKeyboard(message);
                    break;
                case MessageKeyboardState.MinimizedActions:
                    keyboard = new MinimizedActionsKeyboard(message);
                    break;
                case MessageKeyboardState.MaximizedActions:
                    keyboard = new MaximizedActionsKeyboard(message);
                    break;
                case MessageKeyboardState.Attachments:
                    keyboard = new AttachmentsKeyboard(message);
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
