# azure-ingestion-function-app

## Overview
This Azure Function App takes incoming PGP encrypted Zip files and un-encrypts them and extracts them to the destination Azure Data Lake storage location.

It is triggered by EventGrid messages that are generated from Storage Blob activities.

It uses EventGrid event messages instead of direct Storage Blob event messages because blob events are associated with specific containers and would require separate Function Apps for each blob container.

Connections to the storage blobs uses the function apps system assigned managed identity.

The PGP key is stored in Azure Key Vault.

## Coding References

[Unzip Example](https://www.frankysnotes.com/2019/02/how-to-unzip-automatically-your-files.html) and [Conversion Guide](https://elcamino.cloud/articles/2020-03-30-azure-storage-blobs-net-sdk-v12-upgrade-guide-and-tips.html)

[Deserializing Events Delivered to Event Handlers](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/eventgrid/Azure.Messaging.EventGrid/samples/Sample3_ParseAndDeserializeEvents.md)

[Azure Storage Blobs client library for .NET - version 12.15.0](https://learn.microsoft.com/en-us/dotnet/api/overview/azure/storage.blobs-readme?view=azure-dotnet)

[Connect Azure Functions to Azure Storage using Visual Studio Code](https://learn.microsoft.com/en-us/azure/azure-functions/functions-add-output-binding-storage-queue-vs-code?tabs=in-process&pivots=programming-language-csharp)

[Download a blob with .NET](https://learn.microsoft.com/en-us/azure/storage/blobs/storage-blob-download)

