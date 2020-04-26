using System.Collections.Generic;
using System;
using System.Linq;

namespace Runner
{
    public class AlphabeticalLowerProducer : IPasswordProducer
    {
        long counter = 0;
        string _currentValue = "aaa";
        public long CounterValue
        {
            get => counter;
        }

        int _current_length;

        int[] indexes;

        public string CurrentValue
        {
            get=>_currentValue;
            set
            {
                _currentValue = value;
                _current_length = _currentValue.Length;
                List<char> charsetList = charset.ToList();
                indexes = new int[_current_length];
                int i = 0;
                foreach (char c in _currentValue)
                {
                    int index = charsetList.IndexOf(c);
                    indexes[i] = index;
                    i++;
                }
            }
        }
        protected char[] charset ={'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
                                   'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
                                   's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};

        public string GetNextPassword()
        {
            IncrementCounters();
            char[] chars = new char[indexes.Length];
            for(int i=0; i<indexes.Length; i++)
            {
                chars[i] = charset[indexes[i]];
            }
            string ret = new string(chars);
            counter++;
            return ret;
        }

        private void IncrementCounters()
        {
            indexes[0]++;
        }
    }

}