using System.Globalization;
using System;
using System.Linq;

namespace Runner
{
    public class IncrementalNumberProducer : IPasswordProducer
    {
        long counter = 0;

        public long CounterValue 
        { 
            get => counter; 
        }

        public string GetNextPassword()
        {
            var nfi = new NumberFormatInfo();
            nfi.NumberGroupSeparator = "";
            string ret = counter.ToString("N0", nfi);
            counter++;
            return ret;
        }
    }

}