using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage
{
    internal class StoreKeyboard : SendKeyboard
    {
        public StoreKeyboard(NmStoreModel model) : base(model)
        {
        }

        protected override void ButtonsInitializer()
        {
            SaveButton = InitButton(InlineKeyboardType.CallbackData, SaveButtonCaption, SaveButtonCommand);
            ContinueButton = InitButton(InlineKeyboardType.CallbackData, ContinueButtonCaption, ContinueButtonCommand);
            NotSaveButton = InitButton(InlineKeyboardType.CallbackData, NotSaveButtonCaption, NotSaveButtonCommand);

        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            ActionRow = new List<InlineKeyboardButton>();
            if (SaveButton != null)
                ActionRow.Add(SaveButton);
            if (NotSaveButton != null)
                ActionRow.Add(NotSaveButton);
            if (ContinueButton != null)
                ActionRow.Add(ContinueButton);
            
            var inlineKeyboard = new List<List<InlineKeyboardButton>> { ActionRow };
            return inlineKeyboard;
        }

        protected override SendKeyboardState State { get; } = SendKeyboardState.Init;

        protected List<InlineKeyboardButton> ActionRow;


        protected InlineKeyboardButton SaveButton { get; set; }
        protected InlineKeyboardButton NotSaveButton { get; set; }
        protected InlineKeyboardButton ContinueButton { get; set; }


        private static string SaveButtonCaption => SendKeyboardButtonCapton.Save;
        private static string NotSaveButtonCaption => SendKeyboardButtonCapton.NotSave;
        private static string ContinueButtonCaption => SendKeyboardButtonCapton.ContinueOld;


        private static string SaveButtonCommand => CallbackCommand.SAVE_AS_DRAFT_COMMAND;
        private static string NotSaveButtonCommand => CallbackCommand.NOT_SAVE_AS_DRAFT_COMMAND;
        private static string ContinueButtonCommand => CallbackCommand.CONTINUE_COMPOSE_COMMAND;


    }
}