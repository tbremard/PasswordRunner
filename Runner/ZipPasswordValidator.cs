using Ionic.Zip;
using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using Ionic.Zlib;
using System.Linq;

namespace Runner
{
    // 12456 : 45 secs
    internal class ZipPasswordValidator : IPasswordValidator
    {
        string file = "a_123456.zip";
     //   string file = "a_1000.zip";
        string directory = "d:\\zip_test\\";
        string zipPath;
        string extractPath = "d:\\zip_test\\extracted\\";
        MemoryMappedFile _mappedFile;

        public ZipPasswordValidator()
        {
            //zipStream = File.OpenRead(zipPath);
            zipPath = Path.Combine(directory, file);
            MapFile(zipPath);
            ClearDirectory(extractPath);
        }

        private void MapFile(string filePath)
        {
            _mappedFile = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open);
        }

        private void ClearDirectory(string path)
        {
            if (!Directory.Exists(path))
                return;
            string[] files = Directory.GetFiles(path);
            foreach(var file in files)
            {
                File.Delete(file);
            }
        }

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
            catch(BadPasswordException)
            {
                ret = false;
            }
            catch(ZlibException)
            {
                ret = false;
            }
            catch(BadCrcException)
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