using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns
{
    public interface IFilesCache
    {
        FileInfo GetFile(string filename);

        byte[] GetData(string filename);

        void Evict(string filename);

        void RefreshEntry(string filename);

        void WriteThrough(byte[] data, string filename, bool cacheData = false);

        void Empty();
    }
}
