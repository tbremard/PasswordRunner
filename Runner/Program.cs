// Password Runner
// Thierry Brémard
// 2020-04-26

using Modules;
using System;
using System.IO;
using System.Diagnostics;
using Runner.Interfaces;

namespace Runner
{
    class Program
    {
        static string file;
        static string binaryFile;
        static string validatorClassName;
        static string producerClassName;
        static string directory;
        static string validatorInput;

        static MyLogger _logger = new MyLogger();

        private static void Scenario1()
        {
            //            file = "corona.zip";
            file = "baba.zip";
            //file = "1000.zip";
            binaryFile = "Modules\\Modules.dll";
            validatorClassName = "Modules.ZipPasswordValidator";
            producerClassName = "Modules.AlphabeticalLowerProducer";
            //producerClassName = "Modules.IncrementalNumberProducer";
            directory = "..\\poc_input_files\\";
            var fileLocation = new FileLocation { Directory = directory, File = file };
            validatorInput = fileLocation.Serialize();
        }

        static void Main(string[] argv)
        {
            LogVersion();
            SetHighPriority();
            Scenario1();
            var producerModule = new ModuleDefinition()  { BinaryFile = binaryFile, ClassName = producerClassName,  Input = null };
            var validatorModule = new ModuleDefinition() { BinaryFile = binaryFile, ClassName = validatorClassName, Input = validatorInput };
            if(!LoadModules(producerModule, validatorModule))
            {
                return;
            }
            int nbProcessors = LoadConfiguration(argv);
            var runner = new Runner(nbProcessors);
            Console.WriteLine("Start run with {0} processors", nbProcessors);
            var report = runner.Run();
            if (!string.IsNullOrEmpty(report.Password))
            {
                SetForegroundColor(ConsoleColor.Green);
                string message = $"Password found: '{report.Password}' for file: '{file}' in {report.Duration}" + Environment.NewLine;
                Console.Write(message);
                string outFile = Path.Combine(directory, "Report.txt");
                File.AppendAllText(outFile, message);
                ResetForegroundColor();
            }
            else
            {
                _logger.Error("Job in error");
            }
            Console.ReadKey();
        }

        private static void SetHighPriority()
        {
            using (Process p = Process.GetCurrentProcess())
            { 
                p.PriorityClass = ProcessPriorityClass.High; 
            }
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
            try
            {
                ServiceLocator.Instance.PasswordProducer = MyFactory.CreateInstance<IPasswordProducer>(producer.BinaryFile, producer.ClassName, producerArgs);
                ServiceLocator.Instance.PasswordValidator = MyFactory.CreateInstance<IPasswordValidator>(validator.BinaryFile, validator.ClassName, validatorArgs);
            }
            catch(Exception e)
            {
                _logger.Error(e.ToString());
                return false;
            }
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
            _logger.Info("Version: "+assemblyName.Version.ToString());
            _logger.Info("CommandLine: " + String.Join(',',Environment.GetCommandLineArgs()));
            _logger.Info("CurrentDirectory: " + Environment.CurrentDirectory);
//            _logger.Info("MachineName: " + Environment.MachineName);
            //_logger.Info("OSVersion: " + Environment.OSVersion);
        }
    }
}
