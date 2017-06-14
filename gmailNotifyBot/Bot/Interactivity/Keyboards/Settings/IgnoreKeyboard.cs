using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class IgnoreKeyboard : SettingsKeyboard
    {
        protected override void ButtonsInitializer()
        {
            ShowIgnoreButton = InitButton(InlineKeyboardType.CallbackData, IgnoreMenuButtonCaption.Show, CallbackCommand.DISPLAY_IGNORE_COMMAND, SelectedOption.Option1);
            AddToIgnoreButton = InitButton(InlineKeyboardType.SwitchInlineQueryCurrentChat, IgnoreMenuButtonCaption.Add, ForceReplyCommand.ADD_TO_IGNORE_COMMAND);
            RemoveFromIgnoreButton = InitButton(InlineKeyboardType.SwitchInlineQueryCurrentChat, IgnoreMenuButtonCaption.Remove, ForceReplyCommand.REMOVE_FROM_IGNORE_COMMAND);
            BackIgnoreButton = InitButton(InlineKeyboardType.CallbackData, GeneralButtonCaption.Back, CallbackCommand.IGNORE_BACK_COMMAND);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            ShowIgnoreRow = new List<InlineKeyboardButton>();
            if (ShowIgnoreButton != null)
                ShowIgnoreRow.Add(ShowIgnoreButton);

            AddToIgnoreRow = new List<InlineKeyboardButton>();
            if (AddToIgnoreButton != null)
                AddToIgnoreRow.Add(AddToIgnoreButton);

            RemoveFromIgnoreRow = new List<InlineKeyboardButton>();
            if (RemoveFromIgnoreButton != null)
                RemoveFromIgnoreRow.Add(RemoveFromIgnoreButton);

            BackIgnoreRow = new List<InlineKeyboardButton>();
            if (BackIgnoreButton != null)
                BackIgnoreRow.Add(BackIgnoreButton);

            var inlineKeyboard = new List<List<InlineKeyboardButton>> { ShowIgnoreRow, AddToIgnoreRow, RemoveFromIgnoreRow, BackIgnoreRow};
            return inlineKeyboard;
        }

        protected InlineKeyboardButton ShowIgnoreButton { get; set; }
        protected InlineKeyboardButton AddToIgnoreButton { get; set; }
        protected InlineKeyboardButton RemoveFromIgnoreButton { get; set; }
        protected InlineKeyboardButton BackIgnoreButton { get; set; }

        protected List<InlineKeyboardButton> ShowIgnoreRow;
        protected List<InlineKeyboardButton> AddToIgnoreRow;
        protected List<InlineKeyboardButton> RemoveFromIgnoreRow;
        protected List<InlineKeyboardButton> BackIgnoreRow;
        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.IgnoreMenu;
    }
}