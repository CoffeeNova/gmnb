//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity.Keyboards.Getmessage
//{
//    internal abstract class KeyboardFactory
//    {
//        public abstract Keyboard CreateKeyboard(FormattedMessage message);
//        public abstract Keyboard CreateKeyboard(FormattedMessage message, int page);

//        public abstract Keyboard CreateKeyboard(FormattedMessage message, bool isIgnored);

//        public abstract Keyboard CreateKeyboard(FormattedMessage message, int page, bool isIgnored);

//        public abstract Keyboard DefineKeyboard(MessageKeyboardState state, FormattedMessage message, int page, bool isIgnored);
//    }

//    internal class MinimizedKeyboardFactory : KeyboardFactory
//    {
//        public override Keyboard CreateKeyboard(FormattedMessage message)
//        {
//            return new MinimizedKeyboard(message);
//        }
//    }


    

//        public override Keyboard CreateKeyboard(FormattedMessage message, int page)
//        {
//            return new MaximizedKeyboard(message, page);
//        }

//        public override Keyboard CreateKeyboard(FormattedMessage message, bool isIgnored)
//        {
//            return new MinimizedActionsKeyboard(message, isIgnored);
//        }

//        public override Keyboard CreateKeyboard(FormattedMessage message, int page, bool isIgnored)
//        {
//            return new MaximizedActionsKeyboard(message, page, isIgnored);
//        }

//        //public override Keyboard DefineKeyboard(MessageKeyboardState state, FormattedMessage message, int page, bool isIgnored)
//        //{
//        //    switch (state)
//        //    {
//        //        case MessageKeyboardState.Minimized:
//        //            return CreateKeyboard(message);
//        //            break;
//        //        case MessageKeyboardState.Maximized:
//        //            return CreateKeyboard()
//        //    }
//        //}
//    }
//}