using System;
using System.Collections.Generic;
using System.IO;

namespace HlyssUI.Utils
{
    static public class Logger
    {
        public static ConsoleColor Color = ConsoleColor.White;
        public static string Name = "App";

        static Queue<string> messages = new Queue<string>();

        public static void Log(string message, bool condition = true)
        {
            if (!condition || true)
                return;

            message = $"{Name}: {message}";
            addDate(ref message);

            messages.Enqueue(message);

            ConsoleColor tempColor = Console.ForegroundColor;
            Console.ForegroundColor = Color;
            Console.WriteLine(message);
            Console.ForegroundColor = tempColor;
        }

        public static void Log(object obj, bool condition = true)
        {
            Log(obj.ToString(), condition);
        }

        public static void SaveLog()
        {
            string filename = $"Log {DateTime.Now.Day.ToString().PadLeft(2, '0')}.{DateTime.Now.Month.ToString().PadLeft(2, '0')}.{DateTime.Now.Year.ToString()}.txt";
            string contents = Environment.NewLine + "Hlyss log file" + Environment.NewLine;

            if (!Directory.Exists("logs"))
                Directory.CreateDirectory("logs");

            for (int i = 0; i < messages.Count; i++)
            {
                contents += messages.Dequeue() + Environment.NewLine;
            }

            File.AppendAllText($"logs/{filename}", contents);
        }

        private static void addDate(ref string message)
        {
            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int second = DateTime.Now.Second;
            int millisecond = DateTime.Now.Millisecond;
            string hourStr = hour.ToString().PadLeft(2, '0');
            string minuteStr = minute.ToString().PadLeft(2, '0');
            string secondStr = second.ToString().PadLeft(2, '0');
            string millisecondStr = millisecond.ToString();

            message += $" [{hourStr}:{minuteStr}:{secondStr}.{millisecondStr}]";
        }
    }
}
