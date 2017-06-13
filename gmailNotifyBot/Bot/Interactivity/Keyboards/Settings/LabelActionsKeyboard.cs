﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class LabelActionsKeyboard : SettingsKeyboard
    {
        public LabelActionsKeyboard(LabelInfo label)
        {
            Label = label;
        }

        protected override void ButtonsInitializer()
        {
            EditLabelActionsButton = InitButton(InlineKeyboardType.SwitchInlineQueryCurrentChat,
                LabelActionsButtonCaption.EditLabel, ForceReplyCommand.EDIT_LABEL_NAME_COMMAND, SelectedOption.Option1);
            RemoveLabelActionsButton = InitButton(InlineKeyboardType.CallbackData,
                LabelActionsButtonCaption.EditLabel, CallbackCommand.REMOVE_LABEL_COMMAND, SelectedOption.Option1);
            BackLabelActionsButton = InitButton(InlineKeyboardType.CallbackData, GeneralButtonCaption.Back, CallbackCommand.LABEL_ACTIONS_BACK_COMMAND, SelectedOption.Option10);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            if (EditLabelActionsButton != null)
                EditRow.Add(EditLabelActionsButton);

            if (RemoveLabelActionsButton != null)
                RemoveRow.Add(RemoveLabelActionsButton);

            if (BackLabelActionsButton != null)
                BackLabelActionsRow.Add(BackLabelActionsButton);
        }

        protected InlineKeyboardButton EditLabelActionsButton { get; set; }
        protected InlineKeyboardButton RemoveLabelActionsButton { get; set; }
        protected InlineKeyboardButton BackLabelActionsButton { get; set; }

        protected List<InlineKeyboardButton> EditRow = new List<InlineKeyboardButton>();
        protected List<InlineKeyboardButton> RemoveRow = new List<InlineKeyboardButton>();
        protected List<InlineKeyboardButton> BackLabelActionsRow = new List<InlineKeyboardButton>();
        protected LabelInfo Label { get; set; }
        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.LabelActions;
    }
}