using Ionic.Zip;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using Ionic.Zlib;
using System.Linq;
using System.Text.Json;
using Modules.Interfaces;

namespace Modules
{

    public class ZipPasswordValidator : IPasswordValidator
    {
        string zipPath;
        string extractPath = "d:\\zip_test\\extracted\\";
        MemoryMappedFile _mappedFile;

        public ZipPasswordValidator(string jsonFileLocation)
        {
            var fileLocation = JsonSerializer.Deserialize<FileLocation>(jsonFileLocation);
            zipPath = Path.Combine(fileLocation.Directory, fileLocation.File);
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

        //private void ClearDirectory(string path)
        //{
        //    if (!Directory.Exists(path))
        //        return;
        //    string[] files = Directory.GetFiles(path);
        //    foreach(var file in files)
        //    {
        //        File.Delete(file);
        //    }
        //}

        public bool IsValidPassword(string password)
        {
            return IsValidPasswordInMemory(password);
            //            return IsValidPasswordInDisk(password);
        }

        public bool IsValidPasswordInDisk(string password)
        {
            bool ret = true;

            var zipFile = ZipFile.Read(zipPath);
            zipFile.Password = password;

            try
            {
                zipFile.ExtractAll(extractPath);
            }
            catch (BadPasswordException)
            {
                ret = false;
            }
            catch (ZlibException)
            {
                ret = false;
            }
            catch (BadCrcException)
            {
                ret = false;
            }
            zipFile.Dispose();
            return ret;
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
            catch (BadPasswordException)
            {
                ret = false;
            }
            catch (BadCrcException)
            {
                ret = false;
            }
            catch (BadReadException)
            {
                ret = false;
            }
            catch (Exception)
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