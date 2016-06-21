using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using CloudPatterns.CacheAside;
using CloudPatterns.FileProvider;
using Ninject;
using CloudPatterns.Dependencies;
using CloudPatterns.ExternalConfig;
using CloudPatterns.Logging;
using log4net;
using CloudPatterns.BusinessRules.Test;

namespace CloudPatterns
{
    class Program
    {
        static void Main(string[] args)
        {
            TestCase bisTest = new TestCase();
            bisTest.RunTests();

            return;

            // Service creation
            IKernel cont = InjectionHelper.CreateDebugContainer();
            IFilesProvider myfiles = cont.Get<IFilesProvider>();
            ISettingStore settings = cont.Get<ISettingStore>();
            LoggerFactory logFac = cont.Get<LoggerFactory>();
            logFac.Configure();
            ILog logger = logFac.GetLogger();

            File.Delete("fs/data.txt");
            myfiles.Create("data.txt", File.ReadAllBytes("data.txt"));

            logger.Debug("TEST HELLO ?!?!?!");

            Console.ReadLine();

            return;
            // Cloud config
            CloudStorageAccount account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer Container = client.GetContainerReference("mycontainer");
            Container.CreateIfNotExists();
            Container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            //IFilesCache cache = new AzureBlobFileCache(Container);
            IFilesCache cache = new InMemoryCache();

            if (Directory.Exists("depot"))
                Directory.Delete("depot");

            //IFilesProvider files = new LocalFilesProvider("depot");
            IFilesProvider files = new AzureBlobFilesProvider(Container);
            files.SetCache(cache);

            byte[] data = File.ReadAllBytes("data.txt");

            files.Create("data.txt", data);

            byte[] dataGet = files.GetFile("data.txt");

            files.Write("data2.txt", data);

            return;
        }
    }
}
