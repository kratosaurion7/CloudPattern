using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns
{
    public class AzureBlobFileCache : IFilesCache
    {
        private HashSet<CacheEntry> cachedFiles;

        private CloudBlobContainer Container;

        public AzureBlobFileCache(string storageManagerConnectionString, string targetContainerName)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(storageManagerConnectionString);

            CloudBlobClient client = account.CreateCloudBlobClient();

            Container = client.GetContainerReference(targetContainerName);
            Container.CreateIfNotExists();
            Container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            cachedFiles = new HashSet<CacheEntry>();
        }

        public AzureBlobFileCache(CloudBlobContainer container)
        {
            Container = container;

            cachedFiles = new HashSet<CacheEntry>();
        }

        public byte[] GetData(string filename)
        {
            CacheEntry found = cachedFiles.SingleOrDefault(p => p.EntryName == filename);

            if(found == null)
            {
                CloudBlockBlob blockBlob = Container.GetBlockBlobReference(filename);
                blockBlob.FetchAttributes();

                CacheEntry newEntry = new CacheEntry();
                newEntry.Data = new MemoryStream();
                newEntry.EntryName = filename;

                blockBlob.DownloadToStream(newEntry.Data);

                cachedFiles.Add(newEntry);

                found = newEntry;
            }

            byte[] result = new byte[found.Data.Length];

            found.Data.Read(result, 0, (int)found.Data.Length);

            return result;
        }

        public FileInfo GetFile(string filename)
        {
            CacheEntry found = cachedFiles.SingleOrDefault(p => p.EntryName == filename);

            if (found == null)
            {
                CloudBlockBlob blockBlob = Container.GetBlockBlobReference(filename);
                blockBlob.FetchAttributes();

                byte[] data = new byte[blockBlob.Properties.Length];

                blockBlob.DownloadToByteArray(data, 0);

                CacheEntry newEntry = new CacheEntry();
                newEntry.Data = new MemoryStream(data);
                newEntry.EntryName = filename;

                cachedFiles.Add(newEntry);

                found = newEntry;
            }

            string tempFileName = Path.GetTempFileName();

            FileInfo foundFile = new FileInfo(tempFileName);

            FileStream wfs = foundFile.OpenWrite();
            found.Data.WriteTo(wfs);

            wfs.Close();

            return foundFile;
        }

        public void Evict(string filename)
        {
            CacheEntry evictedEntry = cachedFiles.SingleOrDefault(p => p.EntryName == filename);

            evictedEntry.Data.Dispose();

            cachedFiles.Remove(evictedEntry);
        }

        public void RefreshEntry(string filename)
        {
            CacheEntry refreshedEntry = cachedFiles.SingleOrDefault(p => p.EntryName == filename);

            if (refreshedEntry == null)
                return;

            CloudBlockBlob blockBlob = Container.GetBlockBlobReference(filename);
            blockBlob.FetchAttributes();

            byte[] newData = new byte[blockBlob.Properties.Length];

            MemoryStream dlStream = new MemoryStream();
            blockBlob.DownloadToStream(dlStream);

            refreshedEntry.Data.Close();
            refreshedEntry.Data = dlStream;
        }

        public void WriteThrough(byte[] data, string filename, bool cacheData = false)
        {
            CloudBlockBlob blockBlob = Container.GetBlockBlobReference(filename);

            MemoryStream dataStream = new MemoryStream(data);
            
            blockBlob.UploadFromStream(dataStream);

            if(cacheData)
            {
                CacheEntry newEntry = new CacheEntry();
                newEntry.EntryName = filename;
                newEntry.Data = dataStream;

                cachedFiles.Add(newEntry);
            }
            else
            {
                dataStream.Dispose();
            }
        }

        public void Empty()
        {
            foreach (var item in cachedFiles)
            {
                item.Data.Dispose();
            }

            cachedFiles.Clear();
        }
    }
}
