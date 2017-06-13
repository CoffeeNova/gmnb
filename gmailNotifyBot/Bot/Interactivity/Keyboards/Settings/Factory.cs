using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal interface ISettingsKeyboardFactory
    {
        SettingsKeyboard CreateKeyboard(SettingsKeyboardState state);
    }

    internal class SettingsKeyboardFactory : ISettingsKeyboardFactory
    {
        public SettingsKeyboard CreateKeyboard(SettingsKeyboardState state)
        {
            SettingsKeyboard keyboard;
            switch (state)
            {
                case SettingsKeyboardState.MainMenu:
                    keyboard = new MainMenuKeyboard();
                    break;
               
                default:
                    return null;
            }
            if (keyboard == null)
                return null;
            keyboard.CreateInlineKeyboard();
            return keyboard;
        }
    }

}
