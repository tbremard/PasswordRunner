using Modules.Interfaces;
using System.Text;
using System.Security.Cryptography;

namespace Modules
{
    public class Md5Validator: IPasswordValidator
    {
        string _expectedMd5;
        public Md5Validator(string expectedMd5)
        {
            _expectedMd5 = expectedMd5;
        }

        public bool IsValidPassword(string password)
        {
            string currentMd5 = Md5Hash(password);
            bool ret = currentMd5 == _expectedMd5;
            return ret;
        }

        public string Md5Hash(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            MD5CryptoServiceProvider MD5Provider = new MD5CryptoServiceProvider();
            byte[] bytes = MD5Provider.ComputeHash(new UTF8Encoding().GetBytes(inputString));

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("X2"));

            }
            return sb.ToString();
        }
    }
}