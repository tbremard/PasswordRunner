using Runner.Interfaces;

namespace Runner
{
    public class ServiceLocator
    {
        public static ServiceLocator Instance = new ServiceLocator();
        public IPasswordValidator PasswordValidator { get; set; }
        public IPasswordProducer PasswordProducer { get; internal set; }

        private ServiceLocator()
        {

        }
    }
}
