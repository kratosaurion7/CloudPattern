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
            CloudStorageAccount account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer Container = client.GetContainerReference("mycontainer");
            Container.CreateIfNotExists();
            Container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            IFilesCache cache = new AzureBlobFileCache(Container);

            if(Directory.Exists("depot"))
                Directory.Delete("depot");

            //IFilesProvider files = new LocalFilesProvider("depot");
            IFilesProvider files = new AzureBlobFilesProvider(Container);

            byte[] data = File.ReadAllBytes("data.txt");

            files.Create("data.txt", data);

            byte[] dataGet = files.GetFile("data.txt");

            files.Write("data2.txt", data);

            return;
        }
    }
}
