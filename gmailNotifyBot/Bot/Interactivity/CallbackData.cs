using System;
using System.Reflection;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;

namespace CoffeeJelly.gmailNotifyBot.Bot.Interactivity
{
    public abstract class CallbackData
    {
        protected CallbackData()
        {
            Type.NullInspect(nameof(Type));
        }
        public static T Create<T>(string serializedCallbackData) where T : CallbackData
        {
            try
            {
                var instance = (T)Activator.CreateInstance(typeof(T), serializedCallbackData);
                return instance;
            }
            catch (TargetInvocationException ex)
            {
                throw new CallbackDataException($"Cannot build {nameof(T)}", ex);
            }

        }

        public abstract Type Type { get; }
        public string Command { get; set; } = "";

        public const char SEPARATOR = ':';

    }
}