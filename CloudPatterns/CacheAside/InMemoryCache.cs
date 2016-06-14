using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.CacheAside
{
    public class InMemoryCache : IFilesCache
    {
        private HashSet<CacheEntry> Cache;

        public InMemoryCache()
        {
            Cache = new HashSet<CacheEntry>();
        }

        public void AddFile(string filename, byte[] data)
        {
            MemoryStream dataStream = new MemoryStream(data, 0, data.Length);

            CacheEntry newEntry = new CacheEntry();
            newEntry.EntryName = filename;
            newEntry.Data = dataStream;

            Cache.Add(newEntry);
        }

        public void Empty()
        {
            foreach (var item in Cache)
            {
                item.Data.Dispose();
            }

            Cache.Clear();
        }

        public void Evict(string filename)
        {
            CacheEntry found = Cache.SingleOrDefault(p => p.EntryName == filename);

            if(found != null)
            {
                found.Data.Dispose();
            }

            Cache.Remove(found);
        }

        public byte[] GetData(string filename)
        {
            CacheEntry found = Cache.SingleOrDefault(p => p.EntryName == filename);

            if(found != null)
            {
                // TODO : Check expiration
                byte[] data = found.Data.ToArray();

                return data;
            }
            else
            {
                return null;
            }
        }

        public void ReplaceEntry(string filename, byte[] newData)
        {
            CacheEntry found = Cache.SingleOrDefault(p => p.EntryName == filename);

            if(found != null)
            {
                byte[] storageResult = newData;

                found.Data.Dispose();

                MemoryStream newDataStream = new MemoryStream(storageResult, 0, storageResult.Length);
                found.Data = newDataStream;
            }
        }
    }
}
