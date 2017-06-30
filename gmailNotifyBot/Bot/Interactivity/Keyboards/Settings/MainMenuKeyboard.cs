using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class MainMenuKeyboard : SettingsKeyboard
    {
        public MainMenuKeyboard(bool notificationsEnabled)
        {
            NotificationsEnabled = notificationsEnabled;
        }
        protected override void ButtonsInitializer()
        {
            LabelsButton = InitButton(InlineKeyboardType.CallbackData, MainMenuButtonCaption.Labels, CallbackCommand.LABELS_MENU_COMMAND, SelectedOption.Option1);
            PermissionsButton = InitButton(InlineKeyboardType.CallbackData, MainMenuButtonCaption.Permissions, CallbackCommand.PERMISSIONS_MENU_COMMAND, SelectedOption.Option2);
            IgnoreButton = InitButton(InlineKeyboardType.CallbackData, MainMenuButtonCaption.Ignore, CallbackCommand.IGNORE_CONTROL_MENU_COMMAND, SelectedOption.Option3);
            NotifyButton = InitButton(InlineKeyboardType.CallbackData, NotifyButtonCaption, NotifyButtonCommand, SelectedOption.Option4);
            AdditionalButton = InitButton(InlineKeyboardType.CallbackData, MainMenuButtonCaption.Additional, CallbackCommand.ADDITIONAL_MENU_COMMAND, SelectedOption.Option5);
            AboutButton = InitButton(InlineKeyboardType.CallbackData, MainMenuButtonCaption.About, CallbackCommand.ABOUT_COMMAND, SelectedOption.Option9);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            LabelsRow = new List<InlineKeyboardButton>();
            if (LabelsButton != null)
                LabelsRow.Add(LabelsButton);
            PermissionsRow = new List<InlineKeyboardButton>();
            if (PermissionsButton != null)
                PermissionsRow.Add(PermissionsButton);
            IgnoreRow = new List<InlineKeyboardButton>();
            if (IgnoreButton != null)
                IgnoreRow.Add(IgnoreButton);
            AdditionalRow = new List<InlineKeyboardButton>();
            if (AdditionalButton != null)
                AdditionalRow.Add(AdditionalButton);
            NotifyRow = new List<InlineKeyboardButton>();
            if (NotifyButton != null)
                NotifyRow.Add(NotifyButton);
            AboutRow = new List<InlineKeyboardButton>();
            if (AboutButton != null)
                AboutRow.Add(AboutButton);
            var inlineKeyboard = new List<List<InlineKeyboardButton>> { LabelsRow, PermissionsRow, IgnoreRow, AdditionalRow, NotifyRow, AboutRow };
            return inlineKeyboard;
        }

        protected InlineKeyboardButton LabelsButton { get; set; }
        protected InlineKeyboardButton PermissionsButton { get; set; }
        protected InlineKeyboardButton IgnoreButton { get; set; }
        protected InlineKeyboardButton AdditionalButton { get; set; }
        protected InlineKeyboardButton NotifyButton { get; set; }
        protected InlineKeyboardButton AboutButton { get; set; }

        protected List<InlineKeyboardButton> LabelsRow;
        protected List<InlineKeyboardButton> PermissionsRow;
        protected List<InlineKeyboardButton> IgnoreRow;
        protected List<InlineKeyboardButton> AdditionalRow;
        protected List<InlineKeyboardButton> NotifyRow;
        protected List<InlineKeyboardButton> AboutRow;

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.MainMenu;

        protected bool NotificationsEnabled { get; set; }

        private string NotifyButtonCaption
            => NotificationsEnabled ? MainMenuButtonCaption.StopNotify : MainMenuButtonCaption.StartNotify;

        private string NotifyButtonCommand
            => NotificationsEnabled ? CallbackCommand.NOTIFY_STOP_COMMAND : CallbackCommand.NOTIFY_START_COMMAND;
    }
}