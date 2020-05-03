namespace Modules.Interfaces
{
    public interface IDataProducer
    {
        string GetNextData();
        public string CurrentValue { get; set; }
    }
}