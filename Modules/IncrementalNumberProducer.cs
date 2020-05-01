using System.Globalization;
using Runner.Interfaces;

namespace Modules
{
    public class IncrementalNumberProducer : IPasswordProducer
    {
        long counter = 0;
        public IncrementalNumberProducer()
        {
            UpdateValue();
        }

        public long CounterValue
        {
            get => counter;
        }
        public string CurrentValue { get; set; }


        public string GetNextPassword()
        {
            UpdateValue();
            counter++;
            return CurrentValue;
        }

        private void UpdateValue()
        {
            var nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = "";
            CurrentValue = counter.ToString("N0", nfi);
        }
    }

}