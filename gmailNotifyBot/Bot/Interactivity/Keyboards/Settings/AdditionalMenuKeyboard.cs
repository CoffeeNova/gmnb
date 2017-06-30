using System.Collections.Generic;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.TelegramBotApiWrapper.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class AdditionalMenuKeyboard : SettingsKeyboard
    {
        public AdditionalMenuKeyboard(UserSettingsModel settings)
        {
            settings.NullInspect(nameof(settings));
            Settings = settings;
        }
        protected override void ButtonsInitializer()
        {
            ReadAfterReceivingButton = InitButton(InlineKeyboardType.CallbackData, ReadAfterReceivingCaption, CallbackCommand.READ_AFTER_RECEIVING_COMMAND, SelectedOption.Option1);
            BackButton = InitButton(InlineKeyboardType.CallbackData, GeneralButtonCaption.Back, CallbackCommand.ADDITIONAL_BACK_COMMAND);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            ReadAfterReceivingRow = new List<InlineKeyboardButton>();
            if (ReadAfterReceivingButton != null)
                ReadAfterReceivingRow.Add(ReadAfterReceivingButton);

            BackRow = new List<InlineKeyboardButton>();
            if (BackButton != null)
                BackRow.Add(BackButton);

            var inlineKeyboard = new List<List<InlineKeyboardButton>> { ReadAfterReceivingRow, BackRow };
            return inlineKeyboard;
        }

        protected InlineKeyboardButton ReadAfterReceivingButton { get; set; }
        protected InlineKeyboardButton BackButton { get; set; }

        protected List<InlineKeyboardButton> ReadAfterReceivingRow;
        protected List<InlineKeyboardButton> BackRow;

        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.AdditionalMenu;

        private string ReadAfterReceivingCaption => Settings.ReadAfterReceiving
            ? AdditionalMenuButtonCaption.ReadAfterReceivingPos
            : AdditionalMenuButtonCaption.ReadAfterReceivingNeg;

        protected UserSettingsModel Settings;
    }
}