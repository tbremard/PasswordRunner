// Password Runner
// Thierry Brémard
// 2020-04-26

using Modules;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Runner.Interfaces;

namespace Runner
{
    class Program
    {
        static MyLogger _logger = new MyLogger();
        static void Main(string[] argv)
        {
            LogVersion();
            SetHighPriority();
            //            string file = "corona.zip";
            string file = "baba.zip";
            //            string file = "1000.zip";
            string directory = LoadModules(file);
            int nbProcessors = LoadConfiguration(argv);
            var runner = new PasswordRunner(nbProcessors);
            Console.WriteLine("Start run with {0} processors", nbProcessors);
            string password = runner.Run();
            if (!string.IsNullOrEmpty(password))
            {
                SetForegroundColor(ConsoleColor.Green);
                string message = $"Password found: '{password}' for file: '{file}'" + Environment.NewLine;
                Console.Write(message);
                string outFile = Path.Combine(directory, "result.txt");
                File.AppendAllText(outFile, message);
                ResetForegroundColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                ResetForegroundColor();
            }
            Console.ReadKey();
        }

        private static void SetHighPriority()
        {
            using (Process p = Process.GetCurrentProcess())
                p.PriorityClass = ProcessPriorityClass.High;
        }

        private static string LoadModules(string file)
        {
            string binaryFile = "Modules.dll";
            string validatorclassName = "Modules.ZipPasswordValidator";
            string producerClassName = "Modules.AlphabeticalLowerProducer";
            string directory = "..\\..\\poc_input_files\\";
            object[] args = new object[] { directory, file };
            ServiceLocator.Instance.PasswordValidator = MyFactory.CreateInstance<IPasswordValidator>(binaryFile, validatorclassName, args);
            ServiceLocator.Instance.PasswordProducer = MyFactory.CreateInstance<IPasswordProducer>(binaryFile, producerClassName, null);
            return directory;
        }

        private static int LoadConfiguration(string[] argv)
        {
            int ret;
            try
            {
                ret = int.Parse(argv[0]);
            }
            catch
            {
                ret = Environment.ProcessorCount;
            }
            return ret;
        }

        private static void SetForegroundColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        private static void ResetForegroundColor()
        {
            Console.ResetColor();
        }

        public static void LogVersion()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            _logger.Info(assemblyName.CodeBase);
            _logger.Info(assemblyName.FullName);
            _logger.Info(assemblyName.Version.ToString());
            _logger.Info("UserName: " + Environment.UserName);
            _logger.Info("CommandLine: " + Environment.CommandLine);
            _logger.Info("CurrentDirectory: " + Environment.CurrentDirectory);
            _logger.Info("MachineName: " + Environment.MachineName);
            _logger.Info("OSVersion: " + Environment.OSVersion);
        }
    }

    class MyLogger
    {
        public void Info(string s)
        {
            Console.WriteLine(s);
        }
    }

    public static class ApplicationLogging
    {
        public static ILoggerFactory LoggerFactory { get; } = new LoggerFactory();
        public static ILogger CreateLogger<T>() =>
          LoggerFactory.CreateLogger<T>();
    }
}
