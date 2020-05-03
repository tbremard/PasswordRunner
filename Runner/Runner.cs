using Modules.Interfaces;
using System;
using System.Diagnostics;

namespace Runner
{
    public class Runner
    {
        void ProgressUpdateEventHandler(object sender, ProgressUpdateEventArgs e)
        {
            string runTime = e.RunTime.ToString();
            Console.Write($"step: {e.Step}, password: '{e.Password}' time: {runTime} sec\r");
        }

        public RunReport Run(int nbProcessors)
        {
            IDataProducer producer = ServiceLocator.Instance.DataProducer;
            IPasswordValidator validator = ServiceLocator.Instance.PasswordValidator;
            var machine = new ExecutionMachine(nbProcessors, validator);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            machine.RegisterNotifier(ProgressUpdateEventHandler);
            int counter = 0;
            do
            {
                string data = producer.GetNextData();
                machine.Execute(data);
                counter++;
            } while (!machine.isPasswordFound);
            machine.Stop();
            stopWatch.Stop();
            TimeSpan elapsed = stopWatch.Elapsed;
            var totalSeconds = (int)elapsed.TotalSeconds;
            Console.WriteLine("");
            Console.WriteLine("Elapsed: {0} sec", totalSeconds);
            var ret = new RunReport() { Password = machine.successPassword, Duration = elapsed};
            return ret;
        }
    }
}
