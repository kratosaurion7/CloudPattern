using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns
{
    public static class IOUtilities
    {
        public static MemoryStream ToMemoryStream(this Stream stream)
        {
            if (stream.Length > Math.Pow(2, 31))
                throw new Exception("Stream is too big to fit inb a memory buffer.");

            byte[] fileData = new byte[stream.Length];

            stream.Read(fileData, 0, (int)stream.Length);

            MemoryStream mem = new MemoryStream(fileData);

            return mem;
        }
    }
}
