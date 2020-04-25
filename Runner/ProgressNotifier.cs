using System;
using System.ComponentModel;

namespace Runner
{
    public delegate void ProgressUpdateEventHandler(object sender, ProgressUpdateEventArgs e);

    public class ProgressUpdateEventArgs : EventArgs
    {
        private int step;
        private TimeSpan runTime;

        public ProgressUpdateEventArgs(int step, TimeSpan runTime)
        {
            this.Step = step;
            this.runTime = runTime;
        }

        public int Step { get => step; set => step = value; }
        public TimeSpan RunTime { get => runTime; set => runTime = value; }
    }

    public class ProgressNotifier
    {
        DateTime birth = DateTime.Now;
        DateTime last = DateTime.Now;
        object mutex = new Object();
        public event ProgressUpdateEventHandler ProgressUpdate;
        public void DisplayStep(int step)
        {
            lock (mutex)
            {
                bool needUpdate = NeedUpdate();
                if (needUpdate)
                {
                    var  span = GetRunningDuration();
                    OnProgressUpdate(step, span);
                    last = DateTime.Now;
                }
            }
        }

        private void OnProgressUpdate(int step, TimeSpan runTime)
        {
            if(ProgressUpdate == null)
            {
                return;
            }
            var args = new ProgressUpdateEventArgs(step, runTime);
            ProgressUpdate(this, args);
        }

        private bool NeedUpdate()
        {
            int sec = GetSecondsEllapsed();
            bool needUpdate = sec >= 1;
            return needUpdate;
        }

        private int GetSecondsEllapsed()
        {
            TimeSpan ellapsed = DateTime.Now - last;
            int ret = (int)ellapsed.TotalSeconds;
            return ret;
        }

        private TimeSpan GetRunningDuration()
        {
            TimeSpan ellapsed = DateTime.Now - birth;
            //int ret = (int)ellapsed.TotalSeconds;
            return ellapsed;
        }
    }
}
