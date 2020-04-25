using System;

namespace Runner
{
    //todo: run in memory + delete *.tmp
    class Program
    {
        static void Main(string[] argv)
        {
            var provider = new IncrementalNumberProducer();
            int nbProcessors = int.Parse(argv[0]);
            var runner = new PasswordRunner(nbProcessors);
            string password = runner.Run(provider);
            if (!string.IsNullOrEmpty(password))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("password found: '{0}'", password);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR");
            }
            //            Console.ReadKey();
        }
    }
}
