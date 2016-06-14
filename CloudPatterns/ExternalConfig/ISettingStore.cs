using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPatterns.ExternalConfig
{
    /// <summary>
    /// This class is used to access a configuration file independant on the storage
    /// location of the config.
    /// </summary>
    public interface ISettingStore
    {
        /// <summary>
        /// Get the version string of the config file. This number changes after each change in the config.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Get all the configuration values.
        /// </summary>
        /// <returns>Dictionary of key/value config value pairs.</returns>
        Dictionary<string, string> FindAll();

        /// <summary>
        /// Get a specific value buy key name.
        /// </summary>
        /// <param name="key">Key name</param>
        /// <returns>Value found</returns>
        string Get(string key);

        /// <summary>
        /// Update a config value.
        /// </summary>
        /// <param name="key">Key of the config value to change</param>
        /// <param name="value">New value</param>
        /// <remarks>This function is so far not implemented, updating the config file
        /// from clients is not trivial.</remarks>
        void Update(string key, string value);
    }
}
