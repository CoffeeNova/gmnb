using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using CoffeeJelly.gmailNotifyBot.Bot;
using CoffeeJelly.gmailNotifyBot.Bot.Extensions;

namespace CoffeeJelly.gmailNotifyBot.Bot
{
    public static class FormattedMessageHelper
    {
        public static IEnumerable<string> FormatTextBody(IEnumerable<BodyForm> body, int min, int max)
        {
            if (body == null)
                return null;
            var textBody = "";
            foreach (var bodyForm in body)
                if (bodyForm.MimeType == "text/plain")
                    textBody += bodyForm.Value;

            if (String.IsNullOrEmpty(textBody))
                return null;

            var formatted = FormatText(textBody);
            return DivideIntoPages(formatted, min, max);
        }

        public static IEnumerable<string> FormatHtmlBody(IEnumerable<BodyForm> body, int min, int max)
        {
            if (body == null)
                return null;
            var htmlBody = "";
            foreach (var bodyForm in body)
                if (bodyForm.MimeType == "text/html")
                    htmlBody += HtmlConvertToPlainText(bodyForm.Value);

            if (String.IsNullOrEmpty(htmlBody))
                return null;

            var formatted = FormatText(htmlBody);
            return DivideIntoPages(formatted, min, max);
        }

        /// <summary>
        /// Divides the text into pages adjusted to telegram message.
        /// Tries to finish pages by detecting new line or space char in range of <paramref name="minChunkLength"/> to <paramref name="maxChunkLength"/>
        /// if possible. Also checks if each left bracket (&lt;) has a pair (&gt;). The brackets used in this method indicate html tags.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="minChunkLength"></param>
        /// <param name="maxChunkLength"></param>
        /// <returns></returns>
        public static IEnumerable<string> DivideIntoPages(string text, int minChunkLength, int maxChunkLength)
        {
            if (minChunkLength < 1)
                throw new ArgumentOutOfRangeException(nameof(maxChunkLength), "Must equals at least 1");

            int floatingLength = 0;
            int chunks = (int)Math.Ceiling(text.Length / (double)minChunkLength);
            var devided = Enumerable.Range(0, chunks).Select(i =>
            {
                //this inner method checks an integrity of last html tag (<sometag> </sometag>) in content and returns content if last tag is holistic
                //and increase floatingLength by floatingGain or by index last left bracket
                var htmlIntegrity = new Func<string, int, string>((content, floatingGain) =>
                {
                    if (floatingGain == 0)
                        throw new InvalidOperationException($"Operation is impossible in this situation, probably wrong {nameof(text)} parameter," +
                                                            $"it has wrong HTML tags or something.");
                    var indexLeft = content.LastIndexOf('<');
                    if (indexLeft == -1)
                    {
                        floatingLength += floatingGain;
                        return content;
                    }
                    var indexRight = content.LastIndexOf('>');
                    if (indexRight > indexLeft)
                    {
                        if (content[indexLeft + 1] == '/')
                        {
                            floatingLength += floatingGain;
                            return content;
                        }
                    }
                    if (indexLeft == 0)
                        throw new InvalidOperationException($"Operation is impossible in this situation, probably wrong {nameof(text)} parameter," +
                                                            $"it has wrong HTML tags or something.");
                    floatingLength += indexLeft;
                    return content.Substring(0, indexLeft);
                });
                if (floatingLength >= text.Length)
                    return null;
                if (floatingLength + maxChunkLength > text.Length)
                {
                    var subText = text.Substring(floatingLength);
                    floatingLength = text.Length;
                    return subText;
                }
                var temp = text.Substring(floatingLength, maxChunkLength);
                string subTemp;
                var newLineIndex = temp.LastIndexOf(Environment.NewLine, StringComparison.Ordinal);//try to find last line break symbolin pagebreaker range (minChunkLength, maxChunkLength)
                if (newLineIndex < minChunkLength)                                                 //if we can't
                {
                    var emptySymbolIndex = temp.LastIndexOf(' ');//then try to find last space char
                    if (emptySymbolIndex < minChunkLength) //we couldn't find it
                    {
                        return htmlIntegrity(temp, maxChunkLength);
                    }
                    subTemp = temp.Substring(0, emptySymbolIndex);
                    return htmlIntegrity(subTemp, emptySymbolIndex + 1);
                }
                subTemp = temp.Substring(0, newLineIndex);
                return htmlIntegrity(subTemp, newLineIndex + Environment.NewLine.Length);
            }).ToList();
            devided.RemoveAll(String.IsNullOrEmpty);
            //devided.ForEach(s =>
            //{
            //    if (s.StartsWith(" ") && !s.StartsWith("  ")) s = s.Remove(0,1);
            //});
            return devided;
        }

        private static string HtmlConvertToPlainText(string text)
        {
            return HtmlToText.ConvertHtml(text);
        }

        private static string FormatText(string text)
        {
            ParseInnerText(ref text);
            ReplaceSymbolsWithHtmlEntities(ref text);
            AddUrlTags(ref text);
            RemoveExtraBlankLines(ref text);
            return text;
        }

        //private string FormatHtml(string text)
        //{
        //    AddUrlTags(ref text);
        //    ParseInnerText(ref text);
        //    HtmlConvertToPlainText(ref text);
        //    ReplaceSymbolsWithHtmlEntities(ref text);
        //    return text;
        //}

        private static void AddUrlTags(ref string text)
        {
            text = text.Replace("*lt;", "<");
            text = text.Replace("*gt;", ">");
        }

        private static void ParseInnerText(ref string text)
        {
            //var htmlDoc = new HtmlDocument();
            //htmlDoc.LoadHtml(text);
            //text = htmlDoc.DocumentNode.InnerText;
        }

        private static void ReplaceSymbolsWithHtmlEntities(ref string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
        }

        private static void RemoveExtraBlankLines(ref string text)
        {
            text = Regex.Replace(text, $"^\\s+$[{Environment.NewLine}]+", Environment.NewLine, RegexOptions.Multiline);
        }

    }
}