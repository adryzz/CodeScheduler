﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeScheduler.Logging
{
    public class Logger
    {
        public static LogSeverity Verbosity;

        public static List<LogData> LoggedData = new List<LogData>();

        public static void Log(LogSeverity severity, string category, string message)
        {
            LoggedData.Add(new LogData { Severity = severity, Category = category, Message = message, Time = DateTime.Now });
            if ((int)Verbosity <= (int)severity)//log to console only if verbosity is lower or equal
            {
                Console.Write("[");
                Console.ForegroundColor = SeverityColor(severity);
                Console.Write(severity.ToString().ToUpper());
                Console.ResetColor();
                Console.WriteLine($"] {DateTime.Now} | {category} | {message.Replace("\n", "").Replace("\r", "")}");
                Debug.WriteLine($"[{severity.ToString().ToUpper()}] {DateTime.Now} | {category} | {message.Replace("\n", "").Replace("\r", "")}");
            }
        }

        private static ConsoleColor SeverityColor(LogSeverity s)
        {
            switch (s)
            {
                case LogSeverity.Trace:
                    {
                        return ConsoleColor.Gray;
                    }
                case LogSeverity.Debug:
                    {
                        return ConsoleColor.White;
                    }
                case LogSeverity.Info:
                    {
                        return ConsoleColor.Green;
                    }
                case LogSeverity.Warning:
                    {
                        return ConsoleColor.DarkYellow;
                    }
                case LogSeverity.Error:
                    {
                        return ConsoleColor.Red;
                    }
                case LogSeverity.Fatal:
                    {
                        return ConsoleColor.DarkRed;
                    }
                default:
                    {
                        return ConsoleColor.White;
                    }
            }
        }
    }

    public enum LogSeverity : int//the values are the corresponding color
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    public struct LogData
    {
        public LogSeverity Severity;
        public string Category;
        public DateTime Time;
        public string Message;
    }
}
