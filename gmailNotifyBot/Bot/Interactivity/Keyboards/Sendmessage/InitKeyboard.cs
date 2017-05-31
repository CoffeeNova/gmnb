using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage
{
    internal class InitKeyboard : SendKeyboard
    {
        public InitKeyboard(NmStoreModel model) : base(model)
        {
        }

        protected override void ButtonsInitializer()
        {
            ToButton = InitButton(InlineKeyboardType.SwitchInlineQueryCurrentChat, ToButtonCaption, ToButtonCommand, false);
            CcButton = InitButton(InlineKeyboardType.SwitchInlineQueryCurrentChat, CcButtonCaption, CcButtonCommand, false);
            BccButton = InitButton(InlineKeyboardType.SwitchInlineQueryCurrentChat, BccButtonCaption, BccButtonCommand, false);
            MessageButton = InitButton(InlineKeyboardType.CallbackData, SubjectCaption, SubjectButtonCommand);
            SubjectButton = InitButton(InlineKeyboardType.CallbackData, MessageButtonCaption, MessageButtonCommand);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            RecipientsRow = new List<InlineKeyboardButton>();
            if (ToButton != null)
                RecipientsRow.Add(ToButton);
            if (CcButton != null)
                RecipientsRow.Add(CcButton);
            if (BccButton != null)
                RecipientsRow.Add(BccButton);

            MessageRow = new List<InlineKeyboardButton>();
            if (MessageButton != null)
                MessageRow.Add(MessageButton);
            if (SubjectButton != null)
                MessageRow.Add(SubjectButton);
            var inlineKeyboard = new List<List<InlineKeyboardButton>> { RecipientsRow, MessageRow };
            if (Model == null)
                return inlineKeyboard;

            AddDynamicButtons(inlineKeyboard);
            return inlineKeyboard;
        }

        private void AddDynamicButtons(List<List<InlineKeyboardButton>> keyboard)
        {
            var iterFunc = new Func<List<string>, List<InlineKeyboardButton>>(collection =>
            {
                if (collection == null || !collection.Any()) return null;

                var buttonRow = new List<InlineKeyboardButton>();
                collection.IndexEach((item, i) =>
                {
                    buttonRow.Add(InitButton(InlineKeyboardType.CallbackData, item, item));
                });
                return buttonRow;
            });
            RemoveToRow = iterFunc(Model.To);
            RemoveCcRow = iterFunc(Model.Cc);
            RemoveBccRow = iterFunc(Model.Bcc);
            if (RemoveToRow != null)
                keyboard.Add(RemoveToRow);
            if (RemoveCcRow != null)
                keyboard.Add(RemoveCcRow);
            if (RemoveBccRow != null)
                keyboard.Add(RemoveBccRow);
        }

        protected override SendKeyboardState State { get; } = SendKeyboardState.Init;

        protected List<InlineKeyboardButton> RecipientsRow;

        protected List<InlineKeyboardButton> MessageRow;

        protected List<InlineKeyboardButton> RemoveToRow;

        protected List<InlineKeyboardButton> RemoveCcRow;

        protected List<InlineKeyboardButton> RemoveBccRow;

        protected InlineKeyboardButton ToButton { get; set; }
        protected InlineKeyboardButton CcButton { get; set; }
        protected InlineKeyboardButton BccButton { get; set; }
        protected InlineKeyboardButton MessageButton { get; set; }
        protected InlineKeyboardButton SubjectButton { get; set; }

        private static string ToButtonCaption => SendButtonCapton.To;
        private static string CcButtonCaption => SendButtonCapton.Cc;
        private static string BccButtonCaption => SendButtonCapton.Bcc;
        private static string SubjectCaption => SendButtonCapton.Subject;
        private static string MessageButtonCaption => SendButtonCapton.Message;

        private static string ToButtonCommand => Commands.TO_RECIPIENTS_INLINE_QUERY_COMMAND;
        private static string CcButtonCommand => Commands.CC_RECIPIENTS_INLINE_QUERY_COMMAND;
        private static string BccButtonCommand => Commands.BCC_RECIPIENTS_INLINE_QUERY_COMMAND;
        private static string SubjectButtonCommand => Commands.ADD_SUBJECT_COMMAND;
        private static string MessageButtonCommand => Commands.ADD_TEXT_MESSAGE_COMMAND;
    }
}