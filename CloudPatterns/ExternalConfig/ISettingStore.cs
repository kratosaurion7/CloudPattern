using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.ExternalConfig
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISettingStore
    {
        string Version { get; }

        Dictionary<string, string> FindAll();

        string Get(string key);

        void Update(string key, string value);
    }
}
