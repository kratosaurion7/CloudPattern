using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CloudPatterns.Logging
{
    public class AzureFileAppender : AppenderSkeleton
    {
        public string ConnectionstringSettingname { get; set; }

        public string ContainerName { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(ConnectionstringSettingname));
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer Container = client.GetContainerReference(ContainerName);
            Container.CreateIfNotExists();
            Container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

        }
    }
}
