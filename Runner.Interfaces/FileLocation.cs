using System.Text.Json;

namespace Modules
{
    public class FileLocation
    {
        public string Directory { get; set; }
        public string File { get; set; }
        public string Serialize()
        {
            var ret = JsonSerializer.Serialize(this);
            return ret;
        }
    }
}