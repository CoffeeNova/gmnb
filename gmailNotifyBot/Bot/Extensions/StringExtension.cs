using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CoffeeJelly.gmailNotifyBot.Bot.Extensions
{
    public static class StringExtension
    {
        public static bool AnyNullOrEmpty(params string[] str)
        {
            return str.Any(s => String.IsNullOrEmpty(s));
        }

        public static string RemoveWhiteSpaces(this string str)
        {
            return Regex.Replace(str, " ", String.Empty);
        }

        public static string[] RemoveWhiteSpaces(this string[] strArr)
        {
            foreach (var str in strArr)
                str.RemoveWhiteSpaces();
            return strArr;
        }

        public static string PathFormatter(this string str)
        {
            if (String.IsNullOrEmpty(str) || str.Last() == '\\')
                return str;
            return str + "\\";
        }

        public static string PathRemoveLastPart(this string str, out string lastPart)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            if (str == String.Empty)
                throw new ArgumentException($"{nameof(str)} must not be empty.");

            str.PathFormatter();
            var splittedStr = str.Split('\\');
            string formattedStr = String.Empty;
            for (int i = 0; i <= splittedStr.Length - 2; i++)
                formattedStr += splittedStr[i].PathFormatter();

            lastPart = splittedStr[splittedStr.Length - 1];
            return formattedStr;
        }

        public static bool EqualsAny(this string str, params string[] patterns)
        {
            foreach (string pattern in patterns)
                if (str.Equals(pattern))
                    return true;
            return false;
        }

        public static bool EqualsAny(this string str, StringComparison comparisonType, params string[] patterns)
        {
            foreach (string pattern in patterns)
                if (str.Equals(pattern, comparisonType))
                    return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="pattern"></param>
        /// <param name="value"></param>
        /// <remarks>Заместить первый <paramref name="pattern"/> на <paramref name="value"/>.</remarks>
        /// <returns></returns>
        public static string ReplaceFirst(this string text, string pattern, string value)
        {
            var regex = new Regex(Regex.Escape(pattern));
            return regex.Replace(text, value, 1, 0);
        }

        public static string ReplaceFirst(this string text, string pattern, string value, RegexOptions options)
        {
            var regex = new Regex(Regex.Escape(pattern), options);
            return regex.Replace(text, value, 1, 0);
        }

        public static int ParseNumber(this string str)
        {
            var regex = new Regex(@"-?\d+");
            var match = regex.Match(str);

            return match.Success ? Int32.Parse(match.Value) : 0;

        }

        public static int? TryParseNumber(this string str)
        {
            var regex = new Regex(@"-?\d+");
            var match = regex.Match(str);
            if (match.Success)
                return Int32.Parse(match.Value);
            return null;
        }

        public static int[] ParseNumbers(this string str)
        {
            return Regex.Matches(str, @"-?\d+").OfType<Match‌>().Select(m => Int32.Parse(m.Value)).ToArray();
        }

        public static string[] ParseNumbersAsStrings(this string str)
        {
            return Regex.Matches(str, @"-?\d+").OfType<Match‌>().Select(m => m.Value).ToArray();
        }

        public static string[] ParseDoubleNumbersAsStrings(this string str)
        {
            return Regex.Matches(str, @"-?\d+\.\d+").OfType<Match‌>().Select(m => m.Value).ToArray();
        }

        public static IEnumerable GetBetween(this string text, char left, string right)
        {
            var values = Regex.Matches(text, $"(?s)(?<={left}).*?(?={right})");
            return values.Cast<string>();
        }

        public static IEnumerable GetBetween(this string text, char left, char right)
        {
            var values = Regex.Matches(text, $"(?s)(?<={left}).*?(?={right})");
            return values.Cast<string>();
        }

        public static string GetBetweenFirst(this string text, char left, string right)
        {
            var value = Regex.Match(text, $"(?s)(?<={left}).*?(?={right})");
            return value.Value;
        }

        public static string GetBetweenFirst(this string text, char left, char right)
        {
            var value = Regex.Match(text, $"(?s)(?<={left}).*?(?={right})");
            return value.Value;
        }

        public static bool StartsWithAny(this string str, params string[] patterns)
        {
            foreach (string pattern in patterns)
                if (str.StartsWith(pattern))
                    return true;
            return false;
        }

        public static bool StartsWithAny(this string str, StringComparison comparisonType, params string[] patterns)
        {
            foreach (string pattern in patterns)
                if (str.StartsWith(pattern, comparisonType))
                    return true;
            return false;
        }

        /// <summary>
        /// Devide string into parts of <paramref name="chunkLength"/> length
        /// </summary>
        /// <param name="text"></param>
        /// <param name="chunkLength"></param>
        /// <returns></returns>
        public static IEnumerable<string> DivideByLength(this string text, int chunkLength)
        {
            if (chunkLength < 1)
                throw new ArgumentOutOfRangeException(nameof(chunkLength), "Must equals at least 1");

            int chunks = (int)Math.Ceiling(text.Length / (double)chunkLength);
            var devided = Enumerable.Range(0, chunks).Select(i =>
            {
                if (i == chunks - 1)
                    return text.Substring(i * chunkLength, text.Length - i * chunkLength);
                return text.Substring(i * chunkLength, chunkLength);
            });
            return devided;
        }

        /// <summary>
        /// Devide text into parts of <paramref name="linesCount"/> length
        /// </summary>
        /// <param name="text"></param>
        /// <param name="linesCount"></param>
        /// <returns></returns>
        public static IEnumerable<string> DivideByLines(this string text, int linesCount)
        {
            if (linesCount < 1)
                throw new ArgumentOutOfRangeException(nameof(linesCount), "Must equals at least 1");

            var endsWithNewLine = text.EndsWith(Environment.NewLine);
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            if (endsWithNewLine)
                lines.RemoveAt(lines.Count - 1);
            int chunks = (int)Math.Ceiling(lines.Count / (double)linesCount);
            var dividedList = Enumerable.Range(0, chunks).Select(i =>
            {
                var part = lines.Skip(i * linesCount).Take(linesCount);
                string str = "";
                foreach (var line in part)
                    str += line + Environment.NewLine;

                if (i == chunks - 1 && !endsWithNewLine)
                    return str.Trim(Environment.NewLine.ToCharArray());
                return str;
            });
            return dividedList;
        }
    }
}
