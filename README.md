![Image](https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSNOVqyYYV7z_gniVmV6PKhucOmEwQMZXb0KXlUhedUzNlNaxLZ)


# AzureStorageProvider

Abstraction for Blob and FileShare to easily switch between the two without having to change extra code.
Included also is a LocalFileSystem provider for testing or if the fileshares are mounted on the local file system



## Included Operations that are most commonly used
````

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

````


