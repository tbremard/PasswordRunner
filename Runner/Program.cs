// Password Runner
// Thierry Brémard
// 2020-04-26

using Modules;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Runner.Interfaces;
using System.Text.Json;

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
//            string file = "baba.zip";
                        string file = "1000.zip";
            string binaryFile = "Modules.dll";
            string validatorClassName = "Modules.ZipPasswordValidator";
            //string producerClassName = "Modules.AlphabeticalLowerProducer";
            string producerClassName = "Modules.IncrementalNumberProducer";
            string directory = "..\\..\\poc_input_files\\";
            var fileLocation = new FileLocation { Directory = directory, File = file };
            string validatorInput = fileLocation.Serialize();

            var producerModule = new ModuleDefinition() { BinaryFile = binaryFile, ClassName = producerClassName, Input = null };
            var validatorModule = new ModuleDefinition() { BinaryFile = binaryFile, ClassName = validatorClassName, Input= validatorInput };
            LoadModules(producerModule, validatorModule);
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

        private static bool LoadModules(ModuleDefinition producer, ModuleDefinition validator)
        {
            object[] validatorArgs = null;
            object[] producerArgs = null;
            if(producer.Input!=null)
            {
                producerArgs = new object[] { producer.Input };
            }
            if (validator.Input != null)
            {
                validatorArgs = new object[] { validator.Input };
            }
            ServiceLocator.Instance.PasswordProducer  = MyFactory.CreateInstance<IPasswordProducer>(producer.BinaryFile, producer.ClassName, producerArgs);
            ServiceLocator.Instance.PasswordValidator = MyFactory.CreateInstance<IPasswordValidator>(validator.BinaryFile, validator.ClassName, validatorArgs);
            return true;
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
  //          _logger.Info(assemblyName.CodeBase);
//            _logger.Info(assemblyName.FullName);
            _logger.Info("Version: "+assemblyName.Version.ToString());
//            _logger.Info("UserName: " + Environment.UserName);
            _logger.Info("CommandLine: " + String.Join(',',Environment.GetCommandLineArgs()));
            _logger.Info("CurrentDirectory: " + Environment.CurrentDirectory);
//            _logger.Info("MachineName: " + Environment.MachineName);
            //_logger.Info("OSVersion: " + Environment.OSVersion);
        }
    }

    public class ModuleDefinition
    {
        public string BinaryFile { get; set; }
        public string ClassName { get; set; }
        public string Input { get; set; }
    }
}
