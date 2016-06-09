using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.CacheAside
{
    /// <summary>
    /// This class can be used to save transfers by keeping a local copy of a file. The copy of the file 
    /// can be retrieved locally rather than doing an expensive query to a database or the filesystem.
    /// </summary>
    public interface IFilesCache
    {
        /// <summary>
        /// Add a file to the cache.
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <param name="data">Data of the file</param>
        void AddFile(string filename, byte[] data);

        /// <summary>
        /// Retrieve the data of the filename.
        /// </summary>
        /// <param name="filename">Target filename.</param>
        /// <returns>Bytes of the file coming from the cache.</returns>
        byte[] GetData(string filename);

        /// <summary>
        /// Removes the file's entry from the cached storage.
        /// </summary>
        /// <param name="filename"></param>
        void Evict(string filename);

        /// <summary>
        /// Clears the local content of the target file and retrieves a new copy from the storage.
        /// </summary>
        /// <param name="filename"></param>
        void ReplaceEntry(string filename, byte[] newData);

        /// <summary>
        /// Drops all entries from the cache.
        /// </summary>
        void Empty();
    }
}
