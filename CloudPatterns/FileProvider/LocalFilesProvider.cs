using System;
using System.IO;

namespace CloudPatterns
{
    internal class LocalFilesProvider : IFilesProvider
    {
        private DirectoryInfo RootPath;

        public LocalFilesProvider()
        {
            RootPath = new DirectoryInfo(Environment.CurrentDirectory);

            RootPath.Create();
        }

        public LocalFilesProvider(string rootDirectoryPath)
        {
            RootPath = new DirectoryInfo(rootDirectoryPath);

            RootPath.Create();
        }

        public void Create(string filePath, byte[] data)
        {
            string destinationPath = Path.Combine(RootPath.FullName, filePath);
            
            BinaryWriter writer = new BinaryWriter(new FileStream(destinationPath, FileMode.CreateNew));
            writer.Write(data, 0, data.Length);
            writer.Close();
        }

        public void Delete(string filePath)
        {
            string destinationPath = Path.Combine(RootPath.FullName, filePath);

            File.Delete(destinationPath);
        }

        public byte[] GetFile(string filePath)
        {
            string destinationPath = Path.Combine(RootPath.FullName, filePath);

            return File.ReadAllBytes(destinationPath);
        }

        public void SetCache(IFilesCache cachingStore)
        {
            throw new NotImplementedException();
        }

        public void Write(string filePath, byte[] data)
        {
            string destinationPath = Path.Combine(RootPath.FullName, filePath);

            BinaryWriter writer = new BinaryWriter(new FileStream(destinationPath, FileMode.Open));
            writer.Write(data, 0, data.Length);
            writer.Close();
        }
    }
}