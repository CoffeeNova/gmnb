namespace CoffeeJelly.TelegramBotApiWrapper.Extensions
{
    public static class IntExtension
    {
        public static bool InRange(this int numb, int min, int max)
        {
            return numb >= min && numb <= max;
        }
    }
}