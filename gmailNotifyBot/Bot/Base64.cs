using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Ionic.Zlib;
using MimeKit;

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

        public static string EncodeUrlSafe(string plainText)
        {
            plainText = plainText.Replace('+', '-');
            plainText = plainText.Replace('/', '_');
            return Encode(plainText);
        }

        public static string DecodeUrlSafe(string base64EncodedData)
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

        public static byte[] EncodeUrlSafeToBytes(string plainText)
        {
            plainText = plainText.Replace('+', '-');
            plainText = plainText.Replace('/', '_');
            return EncodeToBytes(plainText);
        }

        public static byte[] DecodeUrlSafeToBytes(string base64EncodedData)
        {
            base64EncodedData = base64EncodedData.Replace('-', '+');
            base64EncodedData = base64EncodedData.Replace('_', '/');
            return DecodeToBytes(base64EncodedData);
        }

        public static string Encode(byte[] bytes)
        {
            return System.Convert.ToBase64String(bytes);
        }

        public static string EncodeUrlSafe(byte[] bytes)
        {
            var encoded = Encode(bytes);
            encoded = encoded.Replace('+', '-');
            encoded = encoded.Replace('/', '_');
            return encoded;
        }

        public static byte[] DecodeToBytesUrlSafe(string base64EncodedData)
        {
            base64EncodedData = base64EncodedData.Replace('-', '+');
            base64EncodedData = base64EncodedData.Replace('_', '/');
            return System.Convert.FromBase64String(base64EncodedData);
        }


        public static string CompressStack(byte[] buffer)
        {
            using (var ms = new MemoryStream())
            {
                using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    zip.Write(buffer, 0, buffer.Length);
                }

                ms.Position = 0;
                byte[] compressed = new byte[ms.Length];
                ms.Read(compressed, 0, compressed.Length);

                byte[] gzBuffer = new byte[compressed.Length + 4];
                System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
                System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
                return Encode(gzBuffer);
            }
        }

        public static string Decompress(string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (var ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        public static string EncodeUrlSafe(MimeMessage message)
        {
            using (var stream = new MemoryStream())
            {
                message.WriteTo(stream);

                return Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length)
                    .Replace('+', '-')
                    .Replace('/', '_')
                    .Replace("=", "");
            }
        }
    }
}