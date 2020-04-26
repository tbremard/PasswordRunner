// Password Runner
// Thierry Brémard
// 2020-04-26

using System;

namespace Runner
{
    class Program
    {
        static ConsoleColor defaultColor;

        static void Main(string[] argv)
        {
            defaultColor = Console.ForegroundColor;
            var passwordProducer = new IncrementalNumberProducer();
            int nbProcessors = LoadConfiguration(argv);
            var runner = new PasswordRunner(nbProcessors);
            Console.WriteLine("Start run with {0} processors", nbProcessors);
            string password = runner.Run(passwordProducer);
            if (!string.IsNullOrEmpty(password))
            {
                SetForegroundColor(ConsoleColor.Green);
                Console.WriteLine("password found: '{0}'", password);
                ResetForegroundColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
                ResetForegroundColor();
            }
            //            Console.ReadKey();
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
            Console.ForegroundColor = defaultColor;
        }
    }
}
