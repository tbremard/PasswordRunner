// Password Runner
// Thierry Brémard
// 2020-04-26

using Microsoft.Extensions.Logging;
using System;

namespace Runner
{
    public class MyLogger: ILogger
    {
        Func<object, Exception, string> formatter = (x, y) => x.ToString();
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public void Info(string s)
        {
            var eventId = new EventId(0);
            Log(LogLevel.Information,eventId , s, null, formatter);
        }

        public void Error(string s)
        {
            SetForegroundColor(ConsoleColor.Red);
            var eventId = new EventId(0);
            Log(LogLevel.Error, eventId, s, null, formatter);
            ResetForegroundColor();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine($"[{logLevel}] "+ formatter(state,exception));
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
}
