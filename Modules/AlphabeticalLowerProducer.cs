using System.Collections.Generic;
using System.Linq;
using Modules.Interfaces;

namespace Modules
{
    public class AlphabeticalLowerProducer : IDataProducer
    {
        protected char[] charset ={'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i',
                                   'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
                                   's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
        string _currentValue = "aaa";

        public AlphabeticalLowerProducer()
        {
            CurrentValue = "a";
        }
        long counter = 0;
        public long CounterValue
        {
            get => counter;
        }

        int _current_length;

        int[] indexes;

        public string CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                SetLength(_currentValue.Length);
                UpdateIndexes();
            }
        }

        private void UpdateIndexes()
        {
            List<char> charsetList = charset.ToList();
            int i = 0;
            foreach (char c in _currentValue)
            {
                int index = charsetList.IndexOf(c);
                indexes[i] = index;
                i++;
            }
        }

        private void SetLength(int len)
        {
            _current_length = len;
            indexes = new int[_current_length];
        }

        private void ResetIndexes()
        {
            for (int i = 0; i < _current_length; i++)
            {
                indexes[i] = 0;
            }
        }

        public string GetNextData()
        {
            IncrementCounters();
            UpdateCurrentValue();
            counter++;
            return _currentValue;
        }

        private void UpdateCurrentValue()
        {
            char[] chars = new char[indexes.Length];
            for (int i = 0; i < indexes.Length; i++)
            {
                int j = indexes[i];
                chars[i] = charset[j];
            }
            _currentValue = new string(chars);
        }

        private void IncrementCounters()
        {
            int i = indexes.Length - 1;
            bool carryFlag = true;
            bool overflowFlag = false;
            while (i >= 0 && carryFlag)
            {
                if (indexes[i] == charset.Length - 1)
                {
                    indexes[i] = 0;
                    i--;
                    carryFlag = true;
                    if (i == -1)
                    {
                        overflowFlag = true;
                    }
                }
                else
                {
                    indexes[i]++;
                    carryFlag = false;
                }
            }
            if (overflowFlag)
            {
                SetLength(_current_length + 1);
                ResetIndexes();
            }
        }
    }
}