using CloudPatterns.ExternalConfig;
using CloudPatterns.FileProvider;
using Ninject;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudPatterns.Logging;

namespace CloudPatterns.Dependencies
{
    public static class InjectionHelper
    {
        public static IKernel CreateDebugContainer()
        {
            IKernel debugKernel = new StandardKernel();

            // Make sure the files and folders are present to be used by the services
            if (!Directory.Exists("fs"))
                Directory.CreateDirectory("fs");

            if (!File.Exists("fs/DebugSettings.xml"))
            {
                throw new FileNotFoundException("Debug settings not found, make sure to copy the DebugSettings.xml from the project folder to the app working dir.", "fs/DebugSettings.xml");
            }

            // Using constant bindings
            //debugKernel.Bind<IFilesProvider>().ToConstant(files).InSingletonScope();
            //debugKernel.Bind<ISettingStore>().ToConstant(settings).InSingletonScope();
            //debugKernel.Bind<LoggerFactory>().ToConstant(logFactory).InSingletonScope();

            // Or dynamic construction
            debugKernel.Bind<IFilesProvider>().To<LocalFilesProvider>().WithConstructorArgument("rootDirectoryPath", "fs");
            debugKernel.Bind<ISettingStore>().To<XmlFileSettingStore>().WithConstructorArgument("settingsFilename", "DebugSettings.xml");
            debugKernel.Bind<LoggerFactory>().ToSelf();

            return debugKernel;
        }
    }
}
