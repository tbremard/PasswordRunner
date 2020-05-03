using Modules.Interfaces;

namespace Modules
{
    internal class PasswordValidator : IPasswordValidator
    {
        public bool IsValidPassword(string password)
        {
            if (password == "5606")
                return true;
            return false;
        }
    }

}