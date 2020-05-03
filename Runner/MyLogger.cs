// Password Runner
// Thierry Brémard
// 2020-04-26

using System;

namespace Runner
{
    public class MyLogger
    {
        Func<object, Exception, string> formatter = (x, y) => x.ToString();
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public void Info(string s)
        {
            Log(LogLevel.Info, s);
        }

        public void Error(string s)
        {
            SetForegroundColor(ConsoleColor.Red);
            Log(LogLevel.Error, s);
            ResetForegroundColor();
        }

        public void Success(string s)
        {
            SetForegroundColor(ConsoleColor.Green);
            Log(LogLevel.Success, s);
            ResetForegroundColor();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log(LogLevel logLevel, string msg)
        {
            string level = logLevel.ToString().Substring(0, 4);
            Console.WriteLine($"[{level}] "+ msg);
        }

        private static void SetForegroundColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        private static void ResetForegroundColor()
        {
            Console.ResetColor();
        }

    }

    public enum LogLevel
    {
        Error,
        Info,
        Success
    }
}
