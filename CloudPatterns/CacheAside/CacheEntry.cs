using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.CacheAside
{
    public class CacheEntry
    {
        public string EntryName { get; set; }

        public MemoryStream Data { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is CacheEntry)
            {
                var other = (CacheEntry)obj;

                return this.EntryName == other.EntryName;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return this.EntryName.GetHashCode();
        }
    }
}
