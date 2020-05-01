﻿using Runner.Interfaces;
using System;
using System.Diagnostics;

namespace Runner
{
    public class Runner
    {
        readonly ExecutionMachine machine;

        public Runner(int nbProcessors)
        {
            machine = new ExecutionMachine(nbProcessors);
        }

        void ProgressUpdateEventHandler(object sender, ProgressUpdateEventArgs e)
        {
            string runTime = e.RunTime.ToString();
            Console.Write($"step: {e.Step}, password: '{e.Password}' time: {runTime} sec\r");
        }

        public RunReport Run()
        {
            IPasswordProducer producer = ServiceLocator.Instance.PasswordProducer;
            string password;
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            machine.RegisterNotifier(ProgressUpdateEventHandler);
            int counter = 0;
            do
            {
                password = producer.GetNextPassword();
                machine.Execute(password);
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

    public class RunReport
    {
        public TimeSpan Duration { get; internal set; }
        public string Password { get; internal set; }
    }
}