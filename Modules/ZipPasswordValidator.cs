using Ionic.Zip;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text.Json;
using Modules.Interfaces;

namespace Modules
{
    public class ZipPasswordValidator : IPasswordValidator
    {
        MemoryMappedFile _mappedFile;

        public ZipPasswordValidator(string jsonFileLocation)
        {
            var fileLocation = JsonSerializer.Deserialize<FileLocation>(jsonFileLocation);
            string zipPath = Path.Combine(fileLocation.Directory, fileLocation.File);
            if (!File.Exists(zipPath))
            {
                throw new FileNotFoundException(zipPath);
            }
            MapFile(zipPath);
            Console.WriteLine($"Loaded file: {fileLocation.File}");
        }

        private void MapFile(string filePath)
        {
            _mappedFile = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open);
        }

        public bool IsValidPassword(string password)
        {
            return IsValidPasswordInMemory(password);
        }

        public bool IsValidPasswordInMemory(string password)
        {
            Stream zipStream;
            zipStream = CreateMemoryStream();
            zipStream.Position = 0;
            bool ret;
            using (ZipFile z = ZipFile.Read(zipStream))
            {
                ZipEntry zEntry = z.Entries.Where(x => x.UsesEncryption).First();
                MemoryStream tempS = new MemoryStream();
                ret = TryExtract(password, zEntry, tempS);
            }
            zipStream.Dispose();
            return ret;
        }

        private static bool TryExtract(string password, ZipEntry zEntry, MemoryStream tempS)
        {
            bool ret = true;

            try
            {
                zEntry.ExtractWithPassword(tempS, password);
            }
            catch //BadPasswordException, ZlibException, BadCrcException, BadReadException
            {
                ret = false;
            }
            return ret;
        }

        private Stream CreateMemoryStream()
        {
            MemoryMappedViewStream stream;
            lock (_mappedFile)
            {
                stream = _mappedFile.CreateViewStream();
            }
            return stream;
        }
    }
}