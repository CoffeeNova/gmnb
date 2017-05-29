using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoffeeJelly.TelegramBotApiWrapper.Types.General;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Sendmessage
{
    internal class InitKeyboard : SendKeyboard
    {
        public InitKeyboard(FormattedMessage draft) : base(draft)
        {
        }

        protected override void ButtonsInitializer()
        {
            ToButton = InitButton(ToButtonCaption, ToButtonCommand);
            CcButton = InitButton(CcButtonCaption, CcButtonCommand);
            BccButton = InitButton(BccButtonCaption, BccButtonCommand);
            MessageButton = base.InitButton(SubjectCaption, SubjectButtonCommand);
            SubjectButton = base.InitButton(MessageButtonCaption, MessageButtonCommand);
        }

        protected override InlineKeyboardButton InitButton(string text, string command)
        {
            return new InlineKeyboardButton
            {
                Text = text,
                SwitchInlineQueryCurrentChat = command
            };
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
                RecipientsRow.Add(ToButton);
            if (SubjectButton != null)
                RecipientsRow.Add(CcButton);
            var inlineKeyboard = new List<List<InlineKeyboardButton>> { RecipientsRow, MessageRow };
            return inlineKeyboard;
        }

        protected override SendKeyboardState State { get; } = SendKeyboardState.Init;

        protected List<InlineKeyboardButton> RecipientsRow;

        protected List<InlineKeyboardButton> MessageRow;

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

        private static string ToButtonCommand => Commands.RECIPIENTS_INLINE_QUERY_COMMAND;
        private static string CcButtonCommand => Commands.CC_RECIPIENTS_MESSAGE_COMMAND;
        private static string BccButtonCommand => Commands.BCC_RECIPIENTS_MESSAGE_COMMAND;
        private static string SubjectButtonCommand => Commands.ADD_SUBJECT_COMMAND;
        private static string MessageButtonCommand => Commands.ADD_TEXT_MESSAGE_COMMAND;
    }
}