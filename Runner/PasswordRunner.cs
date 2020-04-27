using System;
using System.Diagnostics;

namespace Runner
{
    public class PasswordRunner
    {
        readonly ExecutionMachine machine;

        public PasswordRunner(int nbProcessors)
        {
            machine = new ExecutionMachine(nbProcessors);
        }

        void ProgressUpdateEventHandler(object sender, ProgressUpdateEventArgs e)
        {
            int totalSeconds = (int)e.RunTime.TotalSeconds;
            Console.Write("step: {0} time: {1} sec\r", e.Step, totalSeconds);
        }

        public string Run()
        {
            IPasswordProducer passProducer = ServiceLocator.Instance.PasswordProducer;
            string password;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            machine.RegisterNotifier(ProgressUpdateEventHandler);
            int counter = 0;
            do
            {
                password = passProducer.GetNextPassword();
                machine.Execute(password);
                counter++;
            } while (!machine.isPasswordFound);
            machine.Stop();
            stopWatch.Stop();
            TimeSpan elapsed = stopWatch.Elapsed;
            var totalSeconds = (int)elapsed.TotalSeconds;
            Console.WriteLine("");
            Console.WriteLine("Elapsed: {0} sec", totalSeconds);
            return machine.successPassword;
        }

    }
}
