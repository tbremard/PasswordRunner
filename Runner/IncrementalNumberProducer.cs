using System.Globalization;

namespace Runner
{
    internal class IncrementalNumberProducer : IPasswordProducer
    {
        decimal counter = 0;
        public string GetNextPassword()
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberGroupSeparator = "";
            string ret = counter.ToString("N0", nfi);
            counter++;
            return ret;
        }
    }
}