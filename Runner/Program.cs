using System;

namespace Runner
{
    class Program
    {
        static ConsoleColor defaultColor;

        static void Main(string[] argv)
        {
            defaultColor = Console.ForegroundColor;
            var provider = new IncrementalNumberProducer();
            int nbProcessors = int.Parse(argv[0]);
            var runner = new PasswordRunner(nbProcessors);
            Console.WriteLine("Start run with {0} processors", nbProcessors);
            string password = runner.Run(provider);
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
