using System;

namespace Runner
{
    public delegate void ProgressUpdateEventHandler(object sender, ProgressUpdateEventArgs e);

    public class ProgressUpdateEventArgs : EventArgs
    {
        public int Step { get; set; }
        public string Password { get; set; }
        public TimeSpan RunTime { get; set; }

        public ProgressUpdateEventArgs(int step, string password, TimeSpan runTime)
        {
            this.Step = step;
            this.RunTime = runTime;
            Password = password;
        }
    }
}
