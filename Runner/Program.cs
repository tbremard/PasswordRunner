// Password Runner
// Thierry Brémard
// 2020-04-26

using System;
using System.IO;

namespace Runner
{
    class Program
    {
        static void Main(string[] argv)
        {
//            string file = "corona.zip";
            string file = "baba.zip";
//            string file = "1000.zip";
            string directory = "..\\..\\poc_input_files\\";
            ServiceLocator.Instance.PasswordValidator = new ZipPasswordValidator(directory, file);
            //ServiceLocator.Instance.PasswordProducer = new IncrementalNumberProducer();
            ServiceLocator.Instance.PasswordProducer = new AlphabeticalLowerProducer();
            int nbProcessors = LoadConfiguration(argv);
            var runner = new PasswordRunner(nbProcessors);
            Console.WriteLine("Start run with {0} processors", nbProcessors);
            string password = runner.Run();
            if (!string.IsNullOrEmpty(password))
            {
                SetForegroundColor(ConsoleColor.Green);
                string message = $"Password found: '{password}' for file: '{file}'"+Environment.NewLine;
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
    }
}
