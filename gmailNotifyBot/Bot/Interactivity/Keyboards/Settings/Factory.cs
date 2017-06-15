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
        SettingsKeyboard CreateKeyboard(SettingsKeyboardState state, UserSettingsModel model, IEnumerable<LabelInfo> labels);
    }

    internal class SettingsKeyboardFactory : ISettingsKeyboardFactory
    {
        public SettingsKeyboard CreateKeyboard(SettingsKeyboardState state, UserSettingsModel model = null, IEnumerable<LabelInfo> labels = null)
        {
            SettingsKeyboard keyboard;
            switch (state)
            {
                case SettingsKeyboardState.MainMenu:
                    keyboard = new MainMenuKeyboard();
                    break;
                case SettingsKeyboardState.EditLabelsMenu:
                    keyboard = new EditLabelsKeyboard(labels);
                    break;
                case SettingsKeyboardState.BlackListMenu:
                    keyboard = new BlackListKeyboard(labels, model?.Blacklist.Select(b => new LabelInfo { Name = b.Name, LabelId = b.LabelId }).ToList(), !model.UseWhitelist);
                    break;
                case SettingsKeyboardState.WhiteListMenu:
                    keyboard = new WhiteListKeyboard(labels, model?.Blacklist.Select(b => new LabelInfo { Name = b.Name, LabelId = b.LabelId }).ToList(), model.UseWhitelist);
                    break;
                case SettingsKeyboardState.LabelsMenu:
                    keyboard = new LabelsKeyboard(model.UseWhitelist);
                    break;
                case SettingsKeyboardState.IgnoreMenu:
                    keyboard = new IgnoreKeyboard();
                    break;
                case SettingsKeyboardState.PermissionsMenu:
                    keyboard = new PermissionsKeyboard(model);
                    break;
                case SettingsKeyboardState.LabelActionsMenu:
                    keyboard = new LabelActionsKeyboard();
                    break;
                default:
                    return null;
            }

            keyboard.CreateInlineKeyboard();
            return keyboard;
        }
    }

}
