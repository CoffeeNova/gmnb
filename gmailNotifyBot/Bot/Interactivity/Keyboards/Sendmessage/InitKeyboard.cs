using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot.DataBase.DataBaseModels;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;
using CoffeeJelly.gmailNotifyBot.Bot.Types;
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
            ToButton = InitButton(InlineKeyboardType.SwitchInlineQueryCurrentChat, ToButtonCaption, ToButtonCommand, "", NmStoreUnit.None, -1, false);
            CcButton = InitButton(InlineKeyboardType.SwitchInlineQueryCurrentChat, CcButtonCaption, CcButtonCommand, "", NmStoreUnit.None, -1, false);
            BccButton = InitButton(InlineKeyboardType.SwitchInlineQueryCurrentChat, BccButtonCaption, BccButtonCommand, "", NmStoreUnit.None, -1, false);
            MessageButton = InitButton(InlineKeyboardType.CallbackData, MessageButtonCaption, MessageButtonCommand);
            SubjectButton = InitButton(InlineKeyboardType.CallbackData, SubjectButtonCaption, SubjectButtonCommand);

            if (Model == null)
                return;
            if (true.EqualsAll(true.EqualsAny(Model.To.Any(), Model.Cc.Any()),
                               !string.IsNullOrEmpty(Model.Subject), 
                               !string.IsNullOrEmpty(Model.Message)))
                SendButton = InitButton(InlineKeyboardType.CallbackData, SendButtonCaption, SendButtonCommand);

            if (true.EqualsAny(Model.To.Any(), 
                               Model.Cc.Any(), 
                               Model.File.Any(),
                               !string.IsNullOrEmpty(Model.Subject),
                               !string.IsNullOrEmpty(Model.Message)))
                ToDraftButton = InitButton(InlineKeyboardType.CallbackData, ToDraftButtonCaption, ToDraftButtonCommand);
        }

        protected override IEnumerable<IEnumerable<InlineKeyboardButton>> DefineInlineKeyboard()
        {
            SendRow = new List<InlineKeyboardButton>();
            if (SendButton != null)
                SendRow.Add(SendButton);
            if (ToDraftButton != null)
                SendRow.Add(ToDraftButton);
            RecipientsRow = new List<InlineKeyboardButton>();
            if (ToButton != null)
                RecipientsRow.Add(ToButton);
            if (CcButton != null)
                RecipientsRow.Add(CcButton);
            if (BccButton != null)
                RecipientsRow.Add(BccButton);

            MessageRow = new List<InlineKeyboardButton>();
            if (SubjectButton != null)
                MessageRow.Add(SubjectButton);
            if (MessageButton != null)
                MessageRow.Add(MessageButton);
            var inlineKeyboard = new List<List<InlineKeyboardButton>> { SendRow, RecipientsRow, MessageRow };
            if (Model == null)
                return inlineKeyboard;

            AddDynamicButtons(inlineKeyboard);
            return inlineKeyboard;
        }

        private void AddDynamicButtons(List<List<InlineKeyboardButton>> keyboard)
        {
            var iterFunc = new Func<List<string>, NmStoreUnit, List<InlineKeyboardButton>>((collection, row) =>
            {
                if (collection == null || !collection.Any()) return null;

                var buttonRow = new List<InlineKeyboardButton>();
                collection.IndexEach((item, i) =>
                {
                    buttonRow.Add(InitButton(InlineKeyboardType.CallbackData, $"{Emoji.BLACK_CROSS}{item}", RemoveItemCommand, "", row, i));
                });
                return buttonRow;
            });
            RemoveToRow = iterFunc(Model.To.Select(a => a.Email).ToList(), NmStoreUnit.To);
            RemoveCcRow = iterFunc(Model.Cc.Select(a => a.Email).ToList(), NmStoreUnit.Cc);
            RemoveBccRow = iterFunc(Model.Bcc.Select(a => a.Email).ToList(), NmStoreUnit.Bcc);
            RemoveFileRow = iterFunc(Model.File.Select(f => f.OriginalName).ToList(), NmStoreUnit.File);
            if (RemoveToRow != null)
                keyboard.Add(RemoveToRow);
            if (RemoveCcRow != null)
                keyboard.Add(RemoveCcRow);
            if (RemoveBccRow != null)
                keyboard.Add(RemoveBccRow);
            if (RemoveFileRow != null)
                keyboard.Add(RemoveFileRow);
        }

        private string ChooseSubjectCaption()
        {
            if (Model == null)
                return SendKeyboardButtonCapton.Subject;

            return string.IsNullOrEmpty(Model.Subject)
                ? SendKeyboardButtonCapton.Subject
                : SendKeyboardButtonCapton.ChangeSubject;
        }

        private string ChooseMessageCaption()
        {
            if (Model == null)
                return SendKeyboardButtonCapton.Message;

            return string.IsNullOrEmpty(Model.Message)
                ? SendKeyboardButtonCapton.Message
                : SendKeyboardButtonCapton.ChangeMessage;
        }

        protected override SendKeyboardState State { get; } = SendKeyboardState.Init;

        protected List<InlineKeyboardButton> RecipientsRow;

        protected List<InlineKeyboardButton> MessageRow;

        protected List<InlineKeyboardButton> RemoveToRow;

        protected List<InlineKeyboardButton> RemoveCcRow;

        protected List<InlineKeyboardButton> RemoveBccRow;

        protected List<InlineKeyboardButton> RemoveFileRow;

        protected List<InlineKeyboardButton> SendRow;

        protected InlineKeyboardButton ToButton { get; set; }
        protected InlineKeyboardButton CcButton { get; set; }
        protected InlineKeyboardButton BccButton { get; set; }
        protected InlineKeyboardButton MessageButton { get; set; }
        protected InlineKeyboardButton SubjectButton { get; set; }
        protected InlineKeyboardButton SendButton { get; set; }
        protected InlineKeyboardButton ToDraftButton { get; set; }


        private static string ToButtonCaption => SendKeyboardButtonCapton.To;
        private static string CcButtonCaption => SendKeyboardButtonCapton.Cc;
        private static string BccButtonCaption => SendKeyboardButtonCapton.Bcc;
        private string SubjectButtonCaption => ChooseSubjectCaption();
        private string MessageButtonCaption => ChooseMessageCaption();
        private static string SendButtonCaption => SendKeyboardButtonCapton.Send;
        private static string ToDraftButtonCaption => SendKeyboardButtonCapton.ToDraft;

        private static string ToButtonCommand => InlineQueryCommand.TO_RECIPIENTS_COMMAND + " ";
        private static string CcButtonCommand => InlineQueryCommand.CC_RECIPIENTS_COMMAND + " ";
        private static string BccButtonCommand => InlineQueryCommand.BCC_RECIPIENTS_COMMAND + " ";
        private static string SubjectButtonCommand => CallbackCommand.ADD_SUBJECT_COMMAND;
        private static string MessageButtonCommand => CallbackCommand.ADD_TEXT_MESSAGE_COMMAND;
        private static string RemoveItemCommand => CallbackCommand.REMOVE_ITEM_COMMAND;
        private static string SendButtonCommand => CallbackCommand.SEND_NEW_MESSAGE_COMMAND;
        private static string ToDraftButtonCommand => CallbackCommand.SAVE_AS_DRAFT_COMMAND;
    }
}