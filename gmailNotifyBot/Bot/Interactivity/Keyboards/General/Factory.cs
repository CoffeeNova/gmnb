namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.General
{
    internal interface IGeneralKeyboardFactory
    {
        GeneralKeyboard CreateKeyboard(GeneralKeyboardState state);
    }

    internal class GeneralKeyboardFactory : IGeneralKeyboardFactory
    {
        public GeneralKeyboard CreateKeyboard(GeneralKeyboardState state)
        {
            GeneralKeyboard keyboard;
            switch (state)
            {
                case GeneralKeyboardState.ResumeNotifications:
                    keyboard = new ResumeNotificationsKeyboard();
                    break;
                default:
                    return null;
            }

            keyboard.CreateInlineKeyboard();
            return keyboard;
        }
    }

}
