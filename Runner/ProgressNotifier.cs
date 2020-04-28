using System;

namespace Runner
{
    public class ProgressNotifier
    {
        DateTime birth = DateTime.Now;
        DateTime last = DateTime.Now;
        object mutex = new Object();
        public event ProgressUpdateEventHandler ProgressUpdate;
        private int _notifyPeriodInSec;

        public ProgressNotifier(int notifyPeriodInSec)
        {
            _notifyPeriodInSec = notifyPeriodInSec;
        }

        public void DisplayStep(int step, string value)
        {
            lock (mutex)
            {
                bool needUpdate = NeedUpdate();
                if (needUpdate)
                {
                    var  span = GetRunningDuration();
                    OnProgressUpdate(step, value, span);
                    last = DateTime.Now;
                }
            }
        }

        private void OnProgressUpdate(int step, string password, TimeSpan runTime)
        {
            if(ProgressUpdate == null)
            {
                return;
            }
            var args = new ProgressUpdateEventArgs(step, password, runTime);
            ProgressUpdate(this, args);
        }

        private bool NeedUpdate()
        {
            int sec = GetSecondsEllapsed();
            bool needUpdate = sec >= _notifyPeriodInSec;
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
            return ellapsed;
        }
    }
}
