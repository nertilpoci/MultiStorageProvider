using MultiStorageProvider.Common.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MultiStorageProvider.GoogleDrive.Service.Implementation
{
    public class GoogleDriveProvider : IStorageProvider
    {

        public GoogleDriveProvider()
        {
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
