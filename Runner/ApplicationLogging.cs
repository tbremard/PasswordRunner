// Password Runner
// Thierry Brémard
// 2020-04-26

using Microsoft.Extensions.Logging;

namespace Runner
{
    public static class ApplicationLogging
    {
        public static ILoggerFactory LoggerFactory { get; } = new LoggerFactory();
        public static ILogger CreateLogger<T>() =>
          LoggerFactory.CreateLogger<T>();
    }
}
