using CloudPatterns.FileProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CloudPatterns.ExternalConfig
{
    public class XmlFileSettingStore : ISettingStore
    {
        /*
         * This class assumes the following schema
         * <config>
         *  <setting name="SettingName" value="SettingValue" />
         *  <setting name="SettingName" value="SettingValue" />
         *  <setting name="SettingName" value="SettingValue" />
         * </config>
         * 
         */

        public string Version { get; set; }

        private IFilesProvider SettingsStorage;

        private string SettingsFilename;

        public XmlFileSettingStore(IFilesProvider storage, string settingsFilename)
        {
            SettingsStorage = storage;
            SettingsFilename = settingsFilename;
        }

        public Dictionary<string, string> FindAll()
        {
            // TODO : Check if a new version of the config file exists, if yes retrieve it and then get the value because it might have been changed.
            XElement configFile = GetXmlConfigFile(SettingsFilename);

            return configFile.Elements("setting").ToDictionary(x => x.Attribute("name").Value, x => x.Attribute("value").Value);
        }

        public string Get(string key)
        {
            // TODO : Check if a new version of the config file exists, if yes retrieve it and then get the value because it might have been changed.
            XElement configFile = GetXmlConfigFile(SettingsFilename);

            // ATTENTION, USING THE '?.' null safety operator.
            return configFile.Elements("setting").SingleOrDefault(p => p.Attribute("name").Value == key)?.Attribute("value").Value;
        }

        public void Update(string key, string value)
        {
            // Currentlt no plans to allow services to update the config value, conccurency errors could arise.
        }

        private XElement GetXmlConfigFile(string filename)
        {
            byte[] settingsData = SettingsStorage.GetFile(filename);

            String stringdata = System.Text.Encoding.Default.GetString(settingsData);

            return XElement.Parse(stringdata);
        }
    }
}
