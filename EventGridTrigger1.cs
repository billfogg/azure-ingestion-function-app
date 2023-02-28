// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Azure.Storage.Blobs;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using Azure.Identity;

namespace Company.Function
{
    public static class EventGridTrigger1
    {
        [FunctionName("EventGridTrigger1")]
        public static void Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());

            string destinationUrl = Environment.GetEnvironmentVariable("destinationUrl");
            string destinationContainer = Environment.GetEnvironmentVariable("destinationContainer");

            try
            {
                string blobUrl = eventGridEvent.Data.ToObjectFromJson<StorageBlobCreatedEventData>().Url;
                string delimiter = "/";
                string[] splitUrl = blobUrl.Split(delimiter.ToCharArray());
                string blobFilename = splitUrl[splitUrl.Length - 1];

                if (1 == 1)
                {
                    BlobServiceClient destBlobServiceClient = new BlobServiceClient(new Uri(destinationUrl), new DefaultAzureCredential());
                    BlobContainerClient destBlobContainerClient = destBlobServiceClient.GetBlobContainerClient(destinationContainer);

                    BlobClient srcBlobClient = new BlobClient(new Uri(blobUrl), new DefaultAzureCredential());

                    using (MemoryStream blobMemStream = new MemoryStream())
                    {
                        srcBlobClient.DownloadTo(blobMemStream);

                        using (ZipArchive archive = new ZipArchive(blobMemStream))
                        {
                            foreach (ZipArchiveEntry entry in archive.Entries)
                            {
                                log.LogInformation($"Now processing {entry.FullName}");

                                //Replace all NO digits, letters, or "-" by a "-" Azure storage is specific on valid characters
                                string validName = Regex.Replace(entry.Name, @"[^a-zA-Z0-9\-.]", "-");

                                BlobClient destBlobClient = destBlobContainerClient.GetBlobClient(validName);
                                using (var fileStream = entry.Open())
                                {
                                    destBlobClient.Upload(fileStream);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.LogInformation($"Error! Something went wrong: {ex.Message}");

            }
        }
    }
}
