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

        public static byte[] EncodeToBytes(string plainText)
        {
            return System.Text.Encoding.UTF8.GetBytes(plainText);
        }

        public static byte[] DecodeToBytes(string base64EncodedData)
        {
            return System.Convert.FromBase64String(base64EncodedData);
        }

        public static byte[] EncodeUrlToBytes(string plainText)
        {
            plainText = plainText.Replace('+', '-');
            plainText = plainText.Replace('/', '_');
            return EncodeToBytes(plainText);
        }

        public static byte[] DecodeUrlToBytes(string base64EncodedData)
        {
            base64EncodedData = base64EncodedData.Replace('-', '+');
            base64EncodedData = base64EncodedData.Replace('_', '/');
            return DecodeToBytes(base64EncodedData);
        }

        public static string Encode(byte[] bytes)
        {
            return System.Convert.ToBase64String(bytes);
        }

        public static string EncodeUrl(byte[] bytes)
        {
            var encoded = Encode(bytes);
            encoded = encoded.Replace('+', '-');
            encoded = encoded.Replace('/', '_');
            return encoded;
        }

        public static byte[] DecodeToBytesUrl(string base64EncodedData)
        {
            base64EncodedData = base64EncodedData.Replace('-', '+');
            base64EncodedData = base64EncodedData.Replace('_', '/');
            return System.Convert.FromBase64String(base64EncodedData);
        }
    }
}