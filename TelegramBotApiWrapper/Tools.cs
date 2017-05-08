using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("TelegramBotApiWrapperTests")]
namespace CoffeeJelly.TelegramBotApiWrapper
{
    internal static class Tools
    {
        ///A-Z, a-z, 0-9, _ and - are allowed.
        public static bool OnlyAllowedCharacters1(string str)
        {
            var regex = new Regex(@"^[a-zA-Z0-9_-]*$");
            var match = regex.Match(str);
            return match.Success;
        }

    }

    
}