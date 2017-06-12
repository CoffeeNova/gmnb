using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class LabelsKeyboard : SettingsKeyboard
    {
        public LabelsKeyboard(bool whitelistEnabled)
        {
            WhitelistEnabled = whitelistEnabled;
            BlacklistEnabled = !whitelistEnabled;
        }
        protected override void ButtonsInitializer()
        {
            DisplayLabelsButton = InitButton(LabelsMenuButtonCaption.DisplayLabels, CallbackCommand.DISPLAY_LABELS_COMMAND);
            CreateLabelButton = InitButton(LabelsMenuButtonCaption.CreateNewLabel, CallbackCommand.CREATE_NEW_LABEL_COMMAND);
            RemoveLabelButton = InitButton(LabelsMenuButtonCaption.RemoveLabel, CallbackCommand.REMOVE_LABEL_COMMAND);
            EditLabelButton = InitButton(LabelsMenuButtonCaption.EditLabel, CallbackCommand.EDIT_LABEL_COMMAND);
            WhitelistButton = InitButton(WhitelistButtonCaption, CallbackCommand.WHITELIST_COMMAND);
            BlacklistButton = InitButton(BlacklistButtonCaption, CallbackCommand.BLACKLIST_COMMAND);
            BackLabelsButton = InitButton(GeneralButtonCaption.Back, CallbackCommand.LABELS_BACK_COMMAND);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            DisplayLabelsRow = new List<InlineKeyboardButton>();
            if (DisplayLabelsButton != null)
                DisplayLabelsRow.Add(DisplayLabelsButton);

            CreateLabelRow = new List<InlineKeyboardButton>();
            if (CreateLabelButton != null)
                CreateLabelRow.Add(CreateLabelButton);

            RemoveLabelRow = new List<InlineKeyboardButton>();
            if (RemoveLabelButton != null)
                RemoveLabelRow.Add(RemoveLabelButton);

            EditLabelRow = new List<InlineKeyboardButton>();
            if (EditLabelButton != null)
                EditLabelRow.Add(EditLabelButton);

            WhitelistRow = new List<InlineKeyboardButton>();
            if (WhitelistButton != null)
                WhitelistRow.Add(WhitelistButton);

            BlacklistRow = new List<InlineKeyboardButton>();
            if (BlacklistButton != null)
                BlacklistRow.Add(BlacklistButton);

            BackLabelsRow = new List<InlineKeyboardButton>();
            if (BackLabelsButton != null)
                BackLabelsRow.Add(BackLabelsButton);

            var inlineKeyboard = new List<List<InlineKeyboardButton>>
            {
                DisplayLabelsRow, CreateLabelRow, RemoveLabelRow, EditLabelRow,
                WhitelistRow, BlacklistRow, BackLabelsRow
            };
            return inlineKeyboard;
        }


        protected InlineKeyboardButton DisplayLabelsButton { get; set; }
        protected InlineKeyboardButton CreateLabelButton { get; set; }
        protected InlineKeyboardButton RemoveLabelButton { get; set; }
        protected InlineKeyboardButton EditLabelButton { get; set; }
        protected InlineKeyboardButton WhitelistButton { get; set; }
        protected InlineKeyboardButton BlacklistButton { get; set; }
        protected InlineKeyboardButton BackLabelsButton { get; set; }

        protected List<InlineKeyboardButton> DisplayLabelsRow;
        protected List<InlineKeyboardButton> CreateLabelRow;
        protected List<InlineKeyboardButton> RemoveLabelRow;
        protected List<InlineKeyboardButton> EditLabelRow;
        protected List<InlineKeyboardButton> WhitelistRow;
        protected List<InlineKeyboardButton> BlacklistRow;
        protected List<InlineKeyboardButton> BackLabelsRow;

        protected bool WhitelistEnabled { get; set; }
        protected bool BlacklistEnabled { get; set; }

        private string WhitelistButtonCaption => WhitelistEnabled
            ? LabelsMenuButtonCaption.WhiteListEnabled
            : LabelsMenuButtonCaption.WhiteListDisabled;
        private string BlacklistButtonCaption => BlacklistEnabled
            ? LabelsMenuButtonCaption.BlackListEnabled
            : LabelsMenuButtonCaption.BlackListDisabled;

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.Labels;
    }
}