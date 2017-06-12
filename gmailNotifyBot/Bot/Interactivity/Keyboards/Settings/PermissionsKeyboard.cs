using System.Collections.Generic;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Settings
{
    internal class PermissionsKeyboard : SettingsKeyboard
    {
        public PermissionsKeyboard(string newPermissions)
        {
            NewPermissions = newPermissions;
        }
        protected override void ButtonsInitializer()
        {
            ChangePermissionsButton = InitButton(ChangePermissionsButtonCaption, CallbackCommand.CHANGE_PERMISSIONS_COMMAND);
            RevokePermissionsButton = InitButton(PermissionsMenuButtonCaption.RevokePermissions, CallbackCommand.REVOKE_REPMISSIONS_COMMAND);
            RevokeViaWebButton = InitButton(PermissionsMenuButtonCaption.RevokeViaWeb, CallbackCommand.REVOKE_VIA_WEB_COMMAND);
            BackPermissionsButton = InitButton(GeneralButtonCaption.Back, CallbackCommand.PERMISSIONS_BACK_COMMAND);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            ChangePermissionsRow = new List<InlineKeyboardButton>();
            if (ChangePermissionsButton != null)
                ChangePermissionsRow.Add(ChangePermissionsButton);

            RevokePermissionsRow = new List<InlineKeyboardButton>();
            if (RevokePermissionsButton != null)
                RevokePermissionsRow.Add(RevokePermissionsButton);

            RevokeViaWebRow = new List<InlineKeyboardButton>();
            if (RevokeViaWebButton != null)
                RevokeViaWebRow.Add(RevokeViaWebButton);

            BackPermissionsRow = new List<InlineKeyboardButton>();
            if (BackPermissionsButton != null)
                BackPermissionsRow.Add(BackPermissionsButton);

            var inlineKeyboard = new List<List<InlineKeyboardButton>> { ChangePermissionsRow, RevokePermissionsRow, RevokeViaWebRow, BackPermissionsRow };
            return inlineKeyboard;
        }

        protected InlineKeyboardButton ChangePermissionsButton { get; set; }
        protected InlineKeyboardButton RevokePermissionsButton { get; set; }
        protected InlineKeyboardButton RevokeViaWebButton { get; set; }
        protected InlineKeyboardButton BackPermissionsButton { get; set; }

        protected List<InlineKeyboardButton> ChangePermissionsRow;
        protected List<InlineKeyboardButton> RevokePermissionsRow;
        protected List<InlineKeyboardButton> RevokeViaWebRow;
        protected List<InlineKeyboardButton> BackPermissionsRow;
        protected override SettingsKeyboardState State { get; } = SettingsKeyboardState.Permissions;

        private string NewPermissions { get;}

        private string ChangePermissionsButtonCaption => PermissionsMenuButtonCaption.ChangePermissions + " " + NewPermissions;
    }
}