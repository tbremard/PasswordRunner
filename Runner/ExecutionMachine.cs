using Modules.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Runner
{
    public class ExecutionMachine
    {
        public bool isPasswordFound { get; private set; } = false;
        public string successPassword { get; private set; } = null;
        ActionBlock<string> worker;
        CancellationTokenSource cancel;
        ProgressNotifier notifier = new ProgressNotifier(1);
        int counter = 0;

        public ExecutionMachine(int nbProcessors, IPasswordValidator validator)
        {
            cancel = new CancellationTokenSource();
            const int MIN_PROCESSORS = 1;
            nbProcessors = Math.Max(MIN_PROCESSORS, nbProcessors);
            var option = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = nbProcessors,
                CancellationToken = cancel.Token
            };
            worker = new ActionBlock<string>(
                  data => TryDataProcess(data, validator),
                  option);
        }

        public void RegisterNotifier(ProgressUpdateEventHandler handler)
        {
            notifier.ProgressUpdate += handler;
        }

        internal void Execute(string data)
        {
            FlushQueueIfNeeded();
            worker.Post(data);
        }

        private void FlushQueueIfNeeded()
        {
            while(worker.InputCount>1000)
            {
                Thread.Sleep(100);
            }
        }

        private void TryDataProcess(string data, IPasswordValidator validator)
        {
            notifier.DisplayStep(counter++, data);
            try
            {
                if (validator.IsValidPassword(data))
                {
                    isPasswordFound = true;
                    successPassword = data;
                    cancel.Cancel();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                isPasswordFound = true;
                successPassword = null;
                cancel.Cancel();
            }
        }

        internal void Stop()
        {
            worker.Complete();
            try
            {
                worker.Completion.Wait();
            }
            catch(AggregateException)
            {
                ;
            }
        }
    }
}
