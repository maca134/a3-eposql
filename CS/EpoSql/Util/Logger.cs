#region usings

using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

#endregion

namespace EpoSql.Util
{
    public class Logger
    {
        public enum LogType
        {
            Info,
            Debug,
            Warn,
            Error
        }

        private const string Logfilename = @"logs\eposql.txt";
#if DEBUG
        private static readonly Level LoadLevel = Level.Debug;
#else
        private static readonly Level LoadLevel = Level.Info;
#endif

        static Logger()
        {
            var hierarchy = (Hierarchy) LogManager.GetRepository();

            var patternLayout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
            };
            patternLayout.ActivateOptions();

            var assemLoc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (assemLoc != null)
            {
                var appender = new RollingFileAppender
                {
                    AppendToFile = false,
                    File = Path.Combine(assemLoc, Logfilename),
                    Layout = patternLayout,
                    MaxSizeRollBackups = 10,
                    MaximumFileSize = "1GB",
                    RollingStyle = RollingFileAppender.RollingMode.Size,
                    StaticLogFileName = true,
                    ImmediateFlush = true
                };
                appender.ActivateOptions();

                hierarchy.Root.AddAppender(appender);
            }
            hierarchy.Root.Level = LoadLevel;

            hierarchy.Configured = true;
            Log("Logger Started", LogType.Info, typeof (Logger));
        }

        public static void Log(string message)
        {
            Log(message, LogType.Info, typeof (Logger));
        }

        public static void Log(string message, LogType logType)
        {
            Log(message, logType, typeof (Logger));
        }

        public static void Log(string message, Type type)
        {
            Log(message, LogType.Info, type);
        }

        public static void Log(string message, LogType logType, Type type)
        {
            new Task(() =>
            {
                switch (logType)
                {
                    case LogType.Info:
                        Info(message, type);
                        break;
                    case LogType.Debug:
                        Debug(message, type);
                        break;
                    case LogType.Warn:
                        Warn(message, type);
                        break;
                    case LogType.Error:
                        Error(message, type);
                        break;
                }
            }).Start();
        }

        private static void Info(string message, Type type)
        {
            var log = LogManager.GetLogger(type);
            log.Info(message);
            ConsoleLog(message, ConsoleColor.Cyan);
        }

        private static void Debug(string message, Type type)
        {
            var log = LogManager.GetLogger(type);
            log.Debug(message);
            ConsoleLog(message, ConsoleColor.Gray);
        }

        private static void Warn(string message, Type type)
        {
            var log = LogManager.GetLogger(type);
            log.Warn(message);
            ConsoleLog(message, ConsoleColor.Yellow);
        }

        private static void Error(string message, Type type)
        {
            var log = LogManager.GetLogger(type);
            log.Error(message);
            ConsoleLog(message, ConsoleColor.Red);
        }

        private static void ConsoleLog(string log, ConsoleColor color = ConsoleColor.White)
        {
            var c = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(log);
            Console.ForegroundColor = c;
        }
    }
}