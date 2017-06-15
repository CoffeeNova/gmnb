﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class MainMenuKeyboard : SettingsKeyboard
    {
        protected override void ButtonsInitializer()
        {
            LabelsButton = InitButton(InlineKeyboardType.CallbackData, MainMenuButtonCaption.Labels, CallbackCommand.LABELS_MENU_COMMAND, SelectedOption.Option1);
            PermissionsButton = InitButton(InlineKeyboardType.CallbackData, MainMenuButtonCaption.Permissions, CallbackCommand.PERMISSIONS_MENU_COMMAND, SelectedOption.Option2);
            IgnoreButton = InitButton(InlineKeyboardType.CallbackData, MainMenuButtonCaption.Ignore, CallbackCommand.IGNORE_LIST_MENU_COMMAND, SelectedOption.Option3);
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
            AboutRow = new List<InlineKeyboardButton>();
            if (AboutButton != null)
                AboutRow.Add(AboutButton);
            var inlineKeyboard = new List<List<InlineKeyboardButton>> { LabelsRow, PermissionsRow, IgnoreRow, AboutRow };
            return inlineKeyboard;
        }

        protected InlineKeyboardButton LabelsButton { get; set; }
        protected InlineKeyboardButton PermissionsButton { get; set; }
        protected InlineKeyboardButton IgnoreButton { get; set; }
        protected InlineKeyboardButton AboutButton { get; set; }

        protected List<InlineKeyboardButton> LabelsRow;
        protected List<InlineKeyboardButton> PermissionsRow;
        protected List<InlineKeyboardButton> IgnoreRow;
        protected List<InlineKeyboardButton> AboutRow;

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.MainMenu;
    }
}