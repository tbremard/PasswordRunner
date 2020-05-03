// Password Runner
// Thierry Brémard
// 2020-04-26

using Modules;
using System;
using System.IO;
using System.Diagnostics;
using Modules.Interfaces;

namespace Runner
{
    class Program
    {
        static string file;
        static string binaryFile;
        static string validatorClassName;
        static string producerClassName;
        static string directory = "..\\poc_input_files\\";
        static string validatorInput;

        static MyLogger _logger = new MyLogger();

        private static void ScenarioZip_1()
        {
            //            file = "corona.zip";
            file = "baba.zip";
            //file = "1000.zip";
            binaryFile = "Modules\\Modules.dll";
            validatorClassName = "Modules.ZipPasswordValidator";
            producerClassName = "Modules.AlphabeticalLowerProducer";
            //producerClassName = "Modules.IncrementalNumberProducer";
            var fileLocation = new FileLocation { Directory = directory, File = file };
            validatorInput = fileLocation.Serialize();
        }

        public static string GetModuleDirectory()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string locationDir = Path.GetDirectoryName(assembly.Location);
            const string MODULE_SUBDIRECTORY = "Modules";
            string ret = Path.Combine(locationDir, MODULE_SUBDIRECTORY);
            return ret;
        }

        private static void ScenarioMd5_1()
        {
            string md5_baba   = "21661093E56E24CD60B10092005C4AC7";
            string md5_corona = "8215E48BD370871E71A61118277B6876";
            string md5_aaaaa = "594F803B380A41396ED63DCA39503542";
            binaryFile = "Modules.dll";
            validatorClassName = "Modules.Md5Validator";
            producerClassName  = "Modules.AlphabeticalLowerProducer";
            validatorInput = md5_aaaaa;
        }

        static void Main(string[] argv)
        {
            LogVersion();
            SetHighPriority();
            //            ScenarioZip_1();
            ScenarioMd5_1();
            var producerModule = new ModuleDefinition()  { BinaryFile = binaryFile, ClassName = producerClassName,  Input = null };
            var validatorModule = new ModuleDefinition() { BinaryFile = binaryFile, ClassName = validatorClassName, Input = validatorInput };
            if(!LoadModules(producerModule, validatorModule))
            {
                return;
            }
            int nbProcessors = LoadConfiguration(argv);
            Console.WriteLine("Start run with {0} processors", nbProcessors);
            var runner = new Runner();
            var report = runner.Run(nbProcessors);
            if (!string.IsNullOrEmpty(report.Password))
            {
                string message = $"Password: '{report.Password}' for: '{validatorInput}' in {report.Duration}" + Environment.NewLine;
                _logger.Success(message);
                string outFile = Path.Combine(directory, "Report.txt");
                File.AppendAllText(outFile, message);
            }
            else
            {
                _logger.Error("Job in error");
            }
//            Console.ReadKey();
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
                string file = Path.Combine(GetModuleDirectory(), producer.BinaryFile);
                ServiceLocator.Instance.DataProducer = MyFactory.CreateInstance<IDataProducer>(file, producer.ClassName, producerArgs);
                file = Path.Combine(GetModuleDirectory(), validator.BinaryFile);
                ServiceLocator.Instance.PasswordValidator = MyFactory.CreateInstance<IPasswordValidator>(file, validator.ClassName, validatorArgs);
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

        public static void LogVersion()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName();
            _logger.Info("Version    : "+assemblyName.Version.ToString());
            _logger.Info("CommandLine: " + String.Join(',',Environment.GetCommandLineArgs()));
            _logger.Info("CurrentDir : " + Environment.CurrentDirectory);
            _logger.Info("LocationDir: " + Path.GetDirectoryName(assembly.Location));
        }
    }
}
