using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns
{
    public class AzureBlobFilesProvider : IFilesProvider
    {
        private IFilesCache Cache;
        private CloudBlobContainer Container;

        public AzureBlobFilesProvider(string storageManagerConnectionString, string targetContainerName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(storageManagerConnectionString);

            CloudBlobClient client = account.CreateCloudBlobClient();

            Container = client.GetContainerReference(targetContainerName);
            Container.CreateIfNotExists();
            Container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        }

        public AzureBlobFilesProvider(CloudBlobContainer container)
        {
            Container = container;
        }

        public void Create(string filePath, byte[] data)
        {
            CloudBlockBlob blockBlob = Container.GetBlockBlobReference(filePath);

            blockBlob.UploadFromByteArray(data, 0, data.Length);

            if (Cache != null)
            {
                Cache.AddFile(filePath, data);

                return;
            }
        }

        public void Delete(string filePath)
        {
            CloudBlockBlob blockBlob = Container.GetBlockBlobReference(filePath);

            blockBlob.Delete();

            if(Cache != null)
            {
                Cache.Evict(filePath);
            }
        }

        public byte[] GetFile(string filePath)
        {
            if(Cache != null)
            {
                byte[] cachedResult = Cache.GetData(filePath);

                if(cachedResult != null)
                {
                    return cachedResult;
                }
            }

            CloudBlockBlob blockBlob = Container.GetBlockBlobReference(filePath);
            blockBlob.FetchAttributes();

            byte[] result = new byte[blockBlob.Properties.Length];

            blockBlob.DownloadToByteArray(result, 0);

            return result;
        }

        public void Write(string filePath, byte[] data)
        {
            CloudBlockBlob blockBlob = Container.GetBlockBlobReference(filePath);

            blockBlob.UploadFromByteArray(data, 0, data.Length);

            if (Cache != null)
            {
                Cache.Evict(filePath);

                Cache.AddFile(filePath, data);

                return;
            }

        }

        public void SetCache(IFilesCache cachingStore)
        {
            Cache = cachingStore;
        }
    }
}
