using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.CacheAside
{
    public class LocalFilesCache : IFilesCache
    {
        private DirectoryInfo CacheDir;

        public LocalFilesCache()
        {
            string cwd = Environment.CurrentDirectory;

            CacheDir = new DirectoryInfo(cwd);
        }

        public LocalFilesCache(DirectoryInfo cacheDirectory)
        {
            CacheDir = cacheDirectory;

            if (!CacheDir.Exists)
                CacheDir.Create();
        }

        public void AddFile(string filename, byte[] data)
        {
            BinaryWriter fileWriter = new BinaryWriter(new FileStream(Path.Combine(CacheDir.FullName, filename), FileMode.Create));
            fileWriter.Write(data);
            fileWriter.Flush();
            fileWriter.Close();
        }

        public void Empty()
        {
            var files = CacheDir.EnumerateFiles("*", SearchOption.AllDirectories);

            foreach (var item in files)
            {
                item.Delete();
            }
        }

        public void Evict(string filename)
        {
            string filePath = Path.Combine(CacheDir.FullName, filename);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public byte[] GetData(string filename)
        {
            string filePath = Path.Combine(CacheDir.FullName, filename);

            if (File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            else
            {
                return null;
            }
        }

        public void ReplaceEntry(string filename, byte[] newData)
        {
            string filePath = Path.Combine(CacheDir.FullName, filename);


            File.WriteAllBytes(filePath, newData);
        }
    }
}
