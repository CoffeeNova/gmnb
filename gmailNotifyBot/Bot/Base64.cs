using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public static class Base64
    {
        public static string Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string EncodeUrl(string plainText)
        {
            plainText = plainText.Replace('+', '-');
            plainText = plainText.Replace('/', '_');
            return Encode(plainText);
        }

        public static string DecodeUrl(string base64EncodedData)
        {
            base64EncodedData = base64EncodedData.Replace('-', '+');
            base64EncodedData = base64EncodedData.Replace('_', '/');
            return Decode(base64EncodedData);
        }

    }
}