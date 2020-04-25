using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Runner
{
    public class ExecutionMachine
    {
        public bool isPasswordFound { get; private set; } = false;
        public string successPassword { get; private set; } = null;
        IPasswordValidator validator;
        ActionBlock<string> worker;
        CancellationTokenSource cancel;
        ProgressNotifier notifier = new ProgressNotifier();
        int counter = 0;

        public ExecutionMachine(int nbProcessors)
        {
            cancel = new CancellationTokenSource();
            validator = new ZipPasswordValidator();
            if(nbProcessors <1)
            {
                nbProcessors = Environment.ProcessorCount;
            }
            var option = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = nbProcessors,
                CancellationToken = cancel.Token
            };
            worker = new ActionBlock<string>(
                  password => TryPassword(password, validator),
                  option);
        }

        public void RegisterNotifier(ProgressUpdateEventHandler handler)
        {
            notifier.ProgressUpdate += handler;
        }

        internal void Execute(string password)
        {
            FlushQueueIfNeeded();
            worker.Post(password);
//            TryPassword(password, validator);
        }

        private void FlushQueueIfNeeded()
        {
            while(worker.InputCount>1000)
            {
                Thread.Sleep(100);
            }
        }

        private void TryPassword(string password, IPasswordValidator validator)
        {
            notifier.DisplayStep(counter++);
            if (validator.IsValidPassword(password))
            {
                isPasswordFound = true;
                successPassword = password;
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
