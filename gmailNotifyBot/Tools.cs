using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Globalization;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace CoffeeJelly
{
    public class CoffeeJTools
    {
        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        /// <remarks>http://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding</remarks>
        public static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }

        public static Encoding GetEncoding(Stream stream)
        {
            // Read the BOM
            var bom = new byte[4];
            stream.Read(bom, 0, 4);

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }

        public static string ArchiveDataFileName(string dateFormat, DateTime initDate, DateTime finalDate, string restName)
        {
            return $"{initDate.Date.ToString(dateFormat)}-{finalDate.Date.ToString(dateFormat)} {restName}";
        }

        public static bool ArchiveDataFileNameValidation(string dateFormat, string fileName)
        {
            try
            {
                string shortFileName = fileName.Split('\\').Last();
                string[] splittedByEmpty = shortFileName.Split(' ');
                string dateRange = splittedByEmpty.First();
                string[] splittedToDates = dateRange.Split('-');
                string initDate = splittedToDates.First();
                string finalDate = splittedToDates.Last();

                if (!DateValidation(initDate, dateFormat) ||
                    !DateValidation(finalDate, dateFormat) ||
                    !Path.HasExtension(splittedByEmpty.Last()))
                    throw new Exception("ArchiveDataFileNameValidation fails");

                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool DateValidation(string dateString, string dateFormat)
        {
            DateTime dt;
            return DateTime.TryParseExact(
                    dateString,
                    dateFormat,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out dt);
        }

        public static Task Delay(double milliseconds)
        {
            var tcs = new TaskCompletionSource<bool>();
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += (obj, args) =>
            {
                tcs.TrySetResult(true);
            };
            timer.Interval = milliseconds;
            timer.AutoReset = false;
            timer.Start();
            return tcs.Task;
        }

        public static string MakeExcelReportFileNameFromDataFilePath(string dataFilePath)
        {
            string dataFileName = System.IO.Path.GetFileNameWithoutExtension(dataFilePath);
            return dataFileName + " report.xlsm";
        }

        /// <summary>
        /// Пытается выполнить функцию <paramref name="func"/>, до тех пор пока не выйдет время выполнения <paramref name="attemptTime"/>.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого значения, должен быть nullable</typeparam>
        /// <param name="attemptTime">Время, которое будет ожидаться положительного результата выполнения функции.</param>
        /// <param name="delayBetweenAttempts">Задержка между попытками выполнения.</param>
        /// <param name="func">Исполняемая функция.</param>
        /// <returns></returns>
        public static T AttemptExecuteFunction<T>(int attemptTime, int delayBetweenAttempts, Func<T> func) where T : class
        {
            T result = null;
            var waitingTimer = Delay(attemptTime);
            while (result == null && waitingTimer.Status != TaskStatus.RanToCompletion)
            {
                try
                {
                    result = func();
                }
                catch
                {
                    result = null;
                }
                if (result == null)
                    Thread.Sleep(delayBetweenAttempts);
            }
            return result;
        }
   }

}
