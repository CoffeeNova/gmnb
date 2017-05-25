using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
{
    internal interface IKeyboardFactory
    {
        Keyboard CreateKeyboard(MessageKeyboardState state, FormattedMessage message);
    }

    internal class KeyboardFactory : IKeyboardFactory
    {
        public Keyboard CreateKeyboard(MessageKeyboardState state, FormattedMessage message)
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
            keyboard.CreateInlineKeyboard();
            return keyboard;
        }
    }

}
