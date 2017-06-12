using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage
{
    internal class DraftedKeyboard : SendKeyboard
    {
        public DraftedKeyboard(NmStoreModel model, string draftId) : base(model)
        {
            DraftId = draftId;
        }

        protected override void ButtonsInitializer()
        {
            if (DraftId == null)
                DraftId = "";
            ContinueButton = InitButton(InlineKeyboardType.CallbackData, ContinueButtonCaption, ContinueButtonCommand, DraftId);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            var row = new List<InlineKeyboardButton>();
            if (ContinueButton != null)
                row.Add(ContinueButton);
            return new List<List<InlineKeyboardButton>> { row };
        }

        protected InlineKeyboardButton ContinueButton { get; set; }

        protected string DraftId { get; set; }

        private static string ContinueButtonCaption => SendKeyboardButtonCapton.Continue;

        private static string ContinueButtonCommand => CallbackCommand.CONTINUE_FROM_DRAFT_COMMAND;

        protected override SendKeyboardState State { get; } = SendKeyboardState.Drafted;

    }
}