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
using System.IO;

namespace CloudPatterns.Logging
{
    public class AzureFileAppender : AppenderSkeleton
    {
        public string ConnectionstringSettingname { get; set; }

        public string ContainerName { get; set; }

        private CloudBlobContainer CloudContainer { get; set; }

        protected override void Append(LoggingEvent loggingEvent)
        {
            CloudBlockBlob targetFile = CloudContainer.GetBlockBlobReference(""); // TODO : Get the blob file name

            var res = targetFile.AcquireLease(TimeSpan.FromSeconds(15), null);
            var stream = targetFile.OpenWrite(null, null);

            string renderedMessage = RenderLoggingEvent(loggingEvent);

            StreamWriter w = new StreamWriter(stream);
            w.WriteLine(renderedMessage);
            w.Close();
            stream.Close();
        }

        public override void ActivateOptions()
        {
            base.ActivateOptions();

            if(ConnectionstringSettingname == null)
            {
                ConnectionstringSettingname = "UseDevelopmentStorage=true;";
            }
            if(ContainerName == null)
            {
                ContainerName = "logs";
            }

            CloudStorageAccount account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(ConnectionstringSettingname));
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudContainer = client.GetContainerReference(ContainerName);
            CloudContainer.CreateIfNotExists();
            CloudContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

        }
    }
}
