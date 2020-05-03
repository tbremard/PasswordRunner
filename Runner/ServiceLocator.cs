using Modules.Interfaces;

namespace Runner
{
    public class ServiceLocator
    {
        public static ServiceLocator Instance = new ServiceLocator();
        public IPasswordValidator PasswordValidator { get; set; }
        public IDataProducer DataProducer { get; set; }

        private ServiceLocator()
        {

        }
    }
}
