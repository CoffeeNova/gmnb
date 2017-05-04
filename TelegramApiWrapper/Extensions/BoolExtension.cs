namespace CoffeeJelly.TelegramApiWrapper.Extensions
{
    public static class GeneralExtension
    {
        public static bool EqualsAny<T>(this T subj, params T[] patterns)
        {
            foreach (var pattern in patterns)
                if (subj.Equals(pattern))
                    return true;
            return false;
        }

        public static bool EqualsAll<T>(this T subj, params T[] patterns)
        {
            foreach (var pattern in patterns)
                if (!subj.Equals(pattern))
                    return false;
            return true;
        }
    }
}