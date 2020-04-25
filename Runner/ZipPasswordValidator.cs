using Ionic.Zip;
using System;
using System.IO;
using System.Threading;
using System.IO.MemoryMappedFiles;
using Ionic.Zlib;

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
        MemoryMappedFile mmf;

        public ZipPasswordValidator()
        {
            //zipStream = File.OpenRead(zipPath);
            zipPath = Path.Combine(directory, file);
            MapFile(zipPath);
            ClearDirectory(extractPath);
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

        // time: 19.62 for '1000' in debug
        // time: 1 sec for '1000' in release
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

        //18   sec for '1000' in 'debug'
        //0.77 sec for '1000' in 'release'
        public bool IsValidPasswordInMemory(string password)
        {
            Stream zipStream;
            zipStream = CreateMemoryStream();
            zipStream.Position = 0;
            bool ret = true;
            using (ZipFile z = ZipFile.Read(zipStream))
            {
                foreach (ZipEntry zEntry in z)
                {
                    MemoryStream tempS = new MemoryStream();
                    try
                    {
                        zEntry.ExtractWithPassword(tempS, password);
                    }
                    catch(BadPasswordException)
                    {
                        ret = false;
                    }
                    catch(BadCrcException)
                    {
                        ret = false;
                    }
                    catch(BadReadException)
                    {
                        ret = false;
                    }
                    catch (Exception)
                    {
                        ret = false;
                    }
                }
            }
            return ret;
        }

        private Stream CreateMemoryStream()
        {
            MemoryMappedViewStream stream;
            lock (mmf)
            {
                stream = mmf.CreateViewStream();
            }
            return stream;
        }

        private void  MapFile(string filePath)
        {
            mmf = MemoryMappedFile.CreateFromFile(filePath, FileMode.Open);
        }
    }
}