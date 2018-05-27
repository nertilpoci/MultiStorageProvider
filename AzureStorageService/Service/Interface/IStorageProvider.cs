using AzureStorageService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.Service.Interface
{
    interface IStorageProvider
    {
        Task<bool> DeleteFolder(string name);
        Task<bool> AddFile(byte[] bytes, string name, bool overrideIfExists = false);
        Task<bool> AddFile(Stream stream, string name, bool overrideIfExists = false);
        Task<bool> AddFile(string fileName, string name, bool overrideIfExists = false);
        Task<bool> DeleteFile(string name);
        Task<byte[]> DownloadBytes(string name);
        Task DownloadStream(Stream stream, string name);
        Task<bool> DownloadToFile(string blobName, string outputFileName, bool overrideIfExists = false);
        Task<bool> FileExists(string fileName);
        Task<bool> RenameFile(string originalName, string newName, bool overrideIfExists = false);
        Task<bool> UpdateFile(Stream stream, string name);
        Task<bool> UpdateFile(string fileName, string name);


    }
}
