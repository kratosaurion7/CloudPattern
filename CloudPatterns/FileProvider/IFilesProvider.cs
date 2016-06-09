using System.IO;

namespace CloudPatterns
{
    /// <summary>
    /// Provides an abstraction over the storage medium of files. This class is used to read and write files from an 
    /// arbitrary storage. The files are meant to be accessed with a relative path (relative to wherever the root of the
    /// storage is).
    /// </summary>
    public interface IFilesProvider
    {
        /// <summary>
        /// Create a file with the specified binary data. File must not exist or an exception will be thrown.
        /// </summary>
        /// <param name="filePath">Relative destination path where the file will be created.</param>
        /// <param name="data">Binary data of the file.</param>
        /// <exception cref="IOException">Throw if there is already a file under the specified name.</exception>
        void Create(string filePath, byte[] data);

        /// <summary>
        /// Read a file and return the content as binary data.
        /// </summary>
        /// <param name="filePath">Relative destination path to the target file.</param>
        /// <returns>Returns the binary data of the file, null if file does not exist.</returns>
        byte[] GetFile(string filePath);

        /// <summary>
        /// Write data to the file at the specified path.
        /// </summary>
        /// <param name="filePath">Relative destination file.</param>
        /// <param name="data">Binary data to write.</param>
        void Write(string filePath, byte[] data);

        /// <summary>
        /// Deletes the file at the specified path.
        /// </summary>
        /// <param name="filePath"></param>
        void Delete(string filePath);
    }
}