using AzureStorageService.Helpers;
using AzureStorageService.Models;
using AzureStorageService.Service.Interface;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.Service.Implementation
{
    public class BlobShareProvider : IStorageProvider
    {
        CloudStorageAccount storageAccount;
        CloudBlobClient bloblClient;
        CloudBlobContainer container;
        CloudBlobDirectory directory;

        /// <summary>
        /// Perform storage actions on a specific share, upon a specific directiory
        /// </summary>
        /// <param name="storageAccount">Storage account info</param>
        /// <param name="containerName">The share to peform actions</param>
        /// <param name="baseDirectory">Directory from where to start browsing</param>
        public BlobShareProvider(CloudStorageAccount storageAccount, string containerName,string baseDirecotry="")
        {
            this.storageAccount = storageAccount;// CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            this.bloblClient = storageAccount.CreateCloudBlobClient();
            container = bloblClient.GetContainerReference(containerName);
            directory= container.GetDirectoryReference(baseDirecotry);
            //AsyncHelper.RunSync(async () => await this.container.CreateIfNotExistsAsync());     
        }


        public async Task<bool> FileExists(string fileName)
        {
            return  await directory.GetBlockBlobReference(fileName).ExistsAsync();
        }
        public async Task<bool> AddFile(string fileName, string name, bool overrideIfExists = false)
        {
            var originalFile = directory.GetBlockBlobReference(name);
            if ( await originalFile.ExistsAsync() && !overrideIfExists) return false;
            try
            {
                await originalFile.UploadFromFileAsync(fileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> AddFile(Stream stream, string name, bool overrideIfExists = false)
        {
            var file = directory.GetBlockBlobReference(name);
            if (await file.ExistsAsync() && !overrideIfExists) return false;
            try
            {
                await file.UploadFromStreamAsync(stream);
            }
            catch
            {

                return false;
            }

            return true;

        }
        public async Task<bool> AddFile(byte[] bytes, string name, bool overrideIfExists = false)
        {
            var  originalFile = directory.GetBlockBlobReference(name);
            if (await originalFile.ExistsAsync() && !overrideIfExists) return false;
            try
            {
                await originalFile.UploadFromByteArrayAsync(bytes, 0, bytes.Length);
            }
            catch
            {
                return false;
            }


            return true;

        }

        public async Task<bool> DeleteFile(string name)
        {
            var originalFile = directory.GetBlockBlobReference(name);
            return await originalFile.DeleteIfExistsAsync();
        }

        public async Task<bool> DeleteFolder(string name)
        {
            var folder = directory.GetDirectoryReference(name);
            try
            {
                BlobContinuationToken token = null;
                do
                {
                    var blobs = await ListFolderContent(folder);
                    token = blobs.ContinuationToken;
                    await DeleteList(blobs.Results);

                } while (null != token);

                return true;
            }
            catch 
            {
                return false;
            }
        }
        private async Task<BlobResultSegment> ListFolderContent(CloudBlobDirectory folder)
        {
           return await folder.ListBlobsSegmentedAsync(true, BlobListingDetails.All, 1000, null, new BlobRequestOptions(), new OperationContext());

        }
        private async Task DeleteList(IEnumerable<IListBlobItem> items)
        {
            foreach (CloudBlob item in items)
            {
                await item.DeleteAsync();
            }
        }
        public async Task<bool> DownloadToFile(string blobName, string outputFileName, bool overrideIfExists = false)
        {
            var originalFile = directory.GetBlockBlobReference(blobName);
            if (!await originalFile.ExistsAsync()) return false;
            if (!overrideIfExists && File.Exists(outputFileName)) return false;

            try
            {
                await originalFile.DownloadToFileAsync(outputFileName,FileMode.OpenOrCreate);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task DownloadStream(Stream stream, string name)
        {
            var originalFile = directory.GetBlockBlobReference(name);
            if (! await originalFile.ExistsAsync()) return ;
            await originalFile.DownloadToStreamAsync(stream);
        }
        public async Task<byte[]> DownloadBytes(string name)
        {
            var originalFile = directory.GetBlockBlobReference(name);
            if (!await originalFile.ExistsAsync()) return null;
            byte[] stream = new byte[originalFile.Properties.Length];
            await originalFile.DownloadToByteArrayAsync(stream, 0);
            return stream;
        }
      
      

        public async Task<bool> RenameFile(string originalName, string newName, bool overrideIfExists = false)
        {
            var originalFile = directory.GetBlockBlobReference(originalName);
            if (! await originalFile.ExistsAsync()) return false;
            var newFile = directory.GetBlockBlobReference(newName);
            if ( await newFile.ExistsAsync() && !overrideIfExists) return false;
            await newFile.StartCopyAsync(originalFile);
            await originalFile.DeleteIfExistsAsync();
            return true;

        }

        public async Task<bool> UpdateFile(Stream stream, string name)
        {
            var originalFile = directory.GetBlockBlobReference(name);
            if (!await originalFile.ExistsAsync()) return false;
            await originalFile.UploadFromStreamAsync(stream);
            return true;
        }

        public async Task<bool> UpdateFile(string fileName, string name)
        {
            var originalFile = directory.GetBlockBlobReference(name);
            if (!await originalFile.ExistsAsync()) return false;
            await originalFile.UploadFromFileAsync(fileName);
            return true;

        }   

    }
}
