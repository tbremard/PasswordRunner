namespace Runner.Interfaces
{
    public interface IPasswordProducer
    {
        string GetNextPassword();
        public string CurrentValue { get; set; }

    }
}