using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns
{
    public class LocalFilesCache : IFilesCache
    {
        private HashSet<CacheEntry> cachedFiles;

        public LocalFilesCache()
        {
            cachedFiles = new HashSet<CacheEntry>();
        }

        public FileInfo GetFile(string filename)
        {
            CacheEntry found = cachedFiles.SingleOrDefault(p => p.EntryName == filename);

            if(found == null)
            {
                FileInfo fileSearch = new FileInfo(filename);

                FileStream fs = fileSearch.OpenRead();

                var cachedStream = fs.ToMemoryStream();

                fs.Close();

                CacheEntry newEntry = new CacheEntry() { EntryName = filename, Data = cachedStream };

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

        public byte[] GetData(string filename)
        {
            CacheEntry found = cachedFiles.SingleOrDefault(p => p.EntryName == filename);

            if (found == null)
            {
                FileInfo fileSearch = new FileInfo(filename);

                FileStream fs = fileSearch.OpenRead();

                var cachedStream = fs.ToMemoryStream();

                fs.Close();

                CacheEntry newEntry = new CacheEntry() { EntryName = filename, Data = cachedStream };

                cachedFiles.Add(newEntry);

                found = newEntry;
            }

            byte[] result = new byte[found.Data.Length];

            found.Data.Read(result, 0, (int)found.Data.Length);

            return result;
        }

        public void Evict(string filename)
        {
            CacheEntry evictedEntry = cachedFiles.SingleOrDefault(p => p.EntryName == filename);

            evictedEntry.Data.Dispose();

            cachedFiles.Remove(evictedEntry);
        }

        public void RefreshEntry(string filename)
        {
            CacheEntry cachedFile = cachedFiles.SingleOrDefault(p => p.EntryName == filename);

            if (cachedFile == null)
                return;

            FileInfo localFile = new FileInfo(filename);

            FileStream localStream = localFile.OpenRead();

            cachedFile.Data.Dispose();

            byte[] fileData = new byte[localStream.Length];

            // FileStream can't read a length bigger than a (signed) int 
            if (localStream.Length > (2 ^ 31))
            {
                throw new Exception("File is too big to fit in a memory buffer.");
            }

            localStream.Read(fileData, 0, (int)localStream.Length);

            var cachedStream = new MemoryStream(fileData, 0, (int)localStream.Length, false);

            cachedFile.Data = cachedStream;

            localStream.Close();
        }

        public void WriteThrough(byte[] data, string filename, bool cacheData = false)
        {
            FileStream writeStream = new FileStream(filename, FileMode.Append);

            writeStream.Write(data, 0, data.Length);

            writeStream.Flush();
            writeStream.Close();

            if(cacheData)
            {
                MemoryStream mem = new MemoryStream(data);

                CacheEntry newEntry = new CacheEntry();
                newEntry.EntryName = filename;
                newEntry.Data = mem;

                cachedFiles.Add(newEntry);
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
