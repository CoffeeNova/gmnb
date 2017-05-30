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
            SaveButton = InitButton(SaveButtonCaption, SaveButtonCommand);
            ContinueButton = InitButton(ContinueButtonCaption, ContinueButtonCommand);
            NotSaveButton = InitButton(NotSaveButtonCaption, NotSaveButtonCommand);

        }

        protected override InlineKeyboardButton InitButton(string text, string callbackCommand)
        {
            return new InlineKeyboardButton
            {
                Text = text,
                SwitchInlineQueryCurrentChat = new SendCallbackData(GeneralCallbackData)
                {
                    Command = callbackCommand,
                    MessageId = Model?.MessageId
                }
            };
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


        private static string SaveButtonCaption => SendButtonCapton.Save;
        private static string NotSaveButtonCaption => SendButtonCapton.NotSave;
        private static string ContinueButtonCaption => SendButtonCapton.Continue;


        private static string SaveButtonCommand => Commands.SAVE_AS_DRAFT_COMMAND;
        private static string NotSaveButtonCommand => Commands.NOT_SAVE_AS_DRAFT_COMMAND;
        private static string ContinueButtonCommand => Commands.CONTINUE_COMPOSE_COMMAND;


    }
}