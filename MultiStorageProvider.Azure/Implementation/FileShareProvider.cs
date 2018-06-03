
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;
using MultiStorageProvider.Common.Helpers;
using MultiStorageProvider.Common.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiStorageProvider.Service.Implementation
{
    public class FileShareProvider : IStorageProvider
    {
        CloudStorageAccount storageAccount;
        CloudFileClient fileClient;
        CloudFileDirectory baseDir;

        /// <summary>
        /// Perform storage actions on a specific share, upon a specific directiory
        /// </summary>
        /// <param name="storageAccount">Storage account info</param>
        /// <param name="shareName">The share to peform actions</param>
        /// <param name="baseDirectory">Directory from where to start browsing</param>
        public FileShareProvider(CloudStorageAccount storageAccount, string shareName, string baseDirectory,bool createIfNotExists)
        {
            this.storageAccount = storageAccount;

            this.fileClient = storageAccount.CreateCloudFileClient();
            var share = fileClient.GetShareReference(shareName);
            if(createIfNotExists) AsyncHelper.RunSync(async () => await share.CreateIfNotExistsAsync());
            baseDir = share.GetRootDirectoryReference().GetDirectoryReference(baseDirectory);
            AsyncHelper.RunSync(async () => await baseDir.CreateIfNotExistsAsync());     
        }


        public async Task<bool> FileExists(string fileName)
        {
            return  await baseDir.GetFileReference(fileName).ExistsAsync();
        }
        public async Task<bool> AddFile(string fileName, string name, bool overrideIfExists = false)
        {
            CloudFile originalFile = baseDir.GetFileReference(name);
            if ( await originalFile.ExistsAsync() && !overrideIfExists) return false;
            try
            {
                using (var str = File.OpenRead(fileName))
                {
                  await  originalFile.UploadFromStreamAsync(str);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> AddFile(Stream stream, string name, bool overrideIfExists = false)
        {
            CloudFile file = baseDir.GetFileReference(name);
            if (await file.ExistsAsync() && !overrideIfExists) return false;
            await CreateFoldersFromPath(name);
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
            CloudFile originalFile = baseDir.GetFileReference(name);
            if (await originalFile.ExistsAsync() && !overrideIfExists) return false;
            await CreateFoldersFromPath(name);

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
            CloudFile originalFile = baseDir.GetFileReference(name);
            return await originalFile.DeleteIfExistsAsync();
        }

        public async Task<bool> DownloadToFile(string blobName, string outputFileName, bool overrideIfExists = false)
        {
            CloudFile originalFile = baseDir.GetFileReference(blobName);
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

        public async Task DownloadStream(Stream stream,string name)
        {
            CloudFile originalFile = baseDir.GetFileReference(name);
            if (! await originalFile.ExistsAsync()) return ;
            await originalFile.DownloadToStreamAsync(stream);
        }
        public async Task<byte[]> DownloadBytes(string name)
        {
            //CloudFile originalFile = baseDir.GetFileReference(name);
            //if (!await originalFile.ExistsAsync()) return null;
            //byte[] stream = new byte[0]();
            //await originalFile.DownloadToByteArrayAsync(stream,0);
            return new byte[0]; ;
        }
        private CloudFileDirectory Directory(string path)
        {
            return string.IsNullOrEmpty(path) ? baseDir : baseDir.GetDirectoryReference(path);
        }
        private string GetPath(string directory, string name)
        {
            if (string.IsNullOrEmpty(directory)) return name;
            return $"{directory}/{name}";
        }
        private async Task CreateFoldersFromPath(string path)
        {
            var folders = path.Split('/');
            CloudFileDirectory dir;
            foreach (var folder in folders.Take(folders.Length > 0 ? folders.Length - 1 : 0))
            {
                dir = baseDir.GetDirectoryReference(folder);
                await dir.CreateIfNotExistsAsync();
            }

        }

        public async Task<bool> RenameFile(string originalName, string newName, bool overrideIfExists = false)
        {
            CloudFile originalFile = baseDir.GetFileReference(originalName);
            if (! await originalFile.ExistsAsync()) return false;
            var newFile = baseDir.GetFileReference(newName);
            if ( await newFile.ExistsAsync() && !overrideIfExists) return false;
            await newFile.StartCopyAsync(originalFile);
            await originalFile.DeleteIfExistsAsync();
            return true;

        }

        public async Task<bool> UpdateFile(Stream stream, string name)
        {
            CloudFile originalFile = baseDir.GetFileReference(name);
            if (!await originalFile.ExistsAsync()) return false;
            await originalFile.UploadFromStreamAsync(stream);
            return true;
        }

        public async Task<bool> UpdateFile(string fileName, string name)
        {
            CloudFile originalFile = baseDir.GetFileReference(name);
            if (!await originalFile.ExistsAsync()) return false;
            await originalFile.UploadFromFileAsync(fileName);
            return true;

        }

        public async Task<bool> DeleteFolder(string name)
        {
          var folder=  baseDir.GetDirectoryReference(name);
           return  await folder.DeleteIfExistsAsync();
        }
    }
}
