using Dropbox.Api;
using Dropbox.Api.Files;
using MultiStorageProvider.Common.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MultiStorageProvider.DropBox.Service.Implementation
{
    public class DropBoxProvider : IStorageProvider
    {
        DropboxClient _client;
        string _baseFolder;
        public DropBoxProvider(string accessToken, string baseFolder)
        {
            var httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromMinutes(20)
            };
            var config = new DropboxClientConfig("SimpleTestApp")
            {
                HttpClient = httpClient
            };
            _client = new DropboxClient(accessToken, config);

             FolderMetadata  folder = CreateFolder(baseFolder).Result;
            _baseFolder = baseFolder;
        }
        private async Task<FolderMetadata> CreateFolder( string path)
        {
            var folderArg = new CreateFolderArg(path);
            var folder = await _client.Files.CreateFolderV2Async(folderArg);

            return folder.Metadata;
        }
        public Task<bool> AddFile(byte[] bytes, string name, bool overrideIfExists = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddFile(Stream stream, string name, bool overrideIfExists = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddFile(string fileName, string name, bool overrideIfExists = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteFile(string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteFolder(string name)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> DownloadBytes(string name)
        {
            throw new NotImplementedException();
        }

        public Task DownloadStream(Stream stream, string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DownloadToFile(string blobName, string outputFileName, bool overrideIfExists = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FileExists(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RenameFile(string originalName, string newName, bool overrideIfExists = false)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateFile(Stream stream, string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateFile(string fileName, string name)
        {
            throw new NotImplementedException();
        }
    }
}
