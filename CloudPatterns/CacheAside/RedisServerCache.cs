using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.CacheAside
{
    public class RedisServerCache : IFilesCache
    {
        IDatabase RedisCache;

        public RedisServerCache(IDatabase redisDb)
        {
            RedisCache = redisDb;
        }

        public void AddFile(string filename, byte[] data)
        {
            var result = RedisCache.StringSet(filename, data, null, When.NotExists);
        }

        public void Empty()
        {
            
        }

        public void Evict(string filename)
        {
            RedisCache.StringSet(filename, RedisValue.Null);
        }

        public byte[] GetData(string filename)
        {
            return RedisCache.StringGet(filename);
        }

        public void ReplaceEntry(string filename, byte[] newData)
        {
            RedisCache.StringSet(filename, newData, null, When.Exists);
        }
    }
}
