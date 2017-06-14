using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using System.Linq;
using GmailLabel = Google.Apis.Gmail.v1.Data.Label;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal interface ISettingsKeyboardFactory
    {
        SettingsKeyboard CreateKeyboard(SettingsKeyboardState state, UserSettingsModel model, IEnumerable<ILabelInfo> labels);
    }

    internal class SettingsKeyboardFactory : ISettingsKeyboardFactory
    {
        public SettingsKeyboard CreateKeyboard(SettingsKeyboardState state, UserSettingsModel model = null, IEnumerable<ILabelInfo> labels = null)
        {
            SettingsKeyboard keyboard;
            switch (state)
            {
                case SettingsKeyboardState.MainMenu:
                    keyboard = new MainMenuKeyboard();
                    break;
                case SettingsKeyboardState.EditLabelsList:
                    keyboard = new EditLabelsListKeyboard(labels);
                    break;
                case SettingsKeyboardState.BlackList:
                    keyboard = new BlackListKeyboard(labels, model?.Blacklist.Select(b => new LabelInfo { Name = b.Name, LabelId = b.LabelId } as ILabelInfo).ToList());
                    break;
                case SettingsKeyboardState.WhiteList:
                    keyboard = new WhiteListKeyboard(labels, model?.Blacklist.Select(b => new LabelInfo { Name = b.Name, LabelId = b.LabelId } as ILabelInfo).ToList());
                    break;
                case SettingsKeyboardState.Labels:
                    break;
                case SettingsKeyboardState.Ignore:
                    break;
                case SettingsKeyboardState.Permissions:
                    keyboard = new PermissionsKeyboard(model);
                    break;
                case SettingsKeyboardState.LabelActions:
                    break;
                default:
                    return null;
            }

            keyboard.CreateInlineKeyboard();
            return keyboard;
        }
    }

}
