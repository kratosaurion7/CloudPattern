using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace CloudPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            IFilesCache cache = new AzureBlobFileCache(CloudConfigurationManager.GetSetting("StorageConnectionString"), "mycontainer");

            if(Directory.Exists("depot"))
                Directory.Delete("depot");

            //IFilesProvider files = new LocalFilesProvider("depot");
            IFilesProvider files = new AzureBlobFilesProvider(CloudConfigurationManager.GetSetting("StorageConnectionString"), "mycontainer");

            byte[] data = File.ReadAllBytes("data.txt");

            files.Create("data.txt", data);

            byte[] dataGet = files.GetFile("data.txt");

            files.Write("data2.txt", data);

            return;
        }
    }
}
