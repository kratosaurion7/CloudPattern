using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using CloudPatterns.FileProvider;
using System.IO;
using CloudPatterns.ExternalConfig;

namespace CloudPatterns.Logging
{
    public class LoggerFactory
    {
        private string DefaultLoggerName;
        private string ConfigFileName;

        private IFilesProvider ConfigSource;
        private ISettingStore Settings;

        public LoggerFactory(IFilesProvider configFileSource, ISettingStore settings)
        {
            ConfigSource = configFileSource;
            Settings = settings;

            DefaultLoggerName = settings.Get("default-logger-name");
            if(DefaultLoggerName == null) // Probably going be null in cases where we don't have a settings repo
            {
                DefaultLoggerName = "default";
            }

            ConfigFileName = settings.Get("logger-config-file-name");
            if(ConfigFileName == null)
            {
                ConfigFileName = "App.config";
            }
        }

        public void Configure()
        {
            byte[] configData = ConfigSource.GetFile(ConfigFileName);

            if(configData == null)
            {
                throw new Exception(String.Format("No file found with the name {0}", ConfigFileName));
            }

            MemoryStream configFileData = new MemoryStream(configData);

            XmlConfigurator.Configure(configFileData);
        }

        public ILog GetLogger(string name = null)
        {
            string wantedLoggerName = name == null ? DefaultLoggerName : name;

            return log4net.LogManager.GetLogger(wantedLoggerName);
        }
    }
}
