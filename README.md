![Image](https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSNOVqyYYV7z_gniVmV6PKhucOmEwQMZXb0KXlUhedUzNlNaxLZ)


# MultiStorageProvider

Abstraction for storage systems. 

Currently incuded:

1. Azure File Storage
2. Azure Blbog Storage
3. File System Storage



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


Easily switch between

1. BlobSotrage
2. FileShare
3. Local File System

Just swap the implementation you want to use and everything else remains the same.

````
Blob
// param1: storage account, pass in the storage account already configured.
//param2: container name, name of your contianer
//param3: baseDirectory, a directory to start the operations from. if base directory "baseDir" any request for files will start from //this directory and go down anything above the directory hierarchy won't be found.
//'basedDir\file1': file1 can be found using just file1 without passing in basedir\file1 if not base dire specified the full path is needed
//param4: createContainerIfNotExist: if set to true will create container. If account has no permissions exception will be thrown.

var blobProvidernew BlobShareProvider(CloudStorageAccount.DevelopmentStorageAccount, "mycontainer", "baseDir",true);


File Share
// param1: storage account, pass in the storage account already configured.
//param2: container name, name of your contianer
//param3: baseDirectory, a directory to start the operations from. if base directory "baseDir" any request for files will start from //this directory and go down anything above the directory hierarchy won't be found.
//'basedDir\file1': file1 can be found using just file1 without passing in basedir\file1 if not base dire specified the full path is needed
//param4: createShareIfNotExist: if set to true will create new share. If account has no permissions exception will be thrown.

var fileShareProvider= FileShareProvider(CloudStorageAccount.DevelopmentStorageAccount, "myshare", "baseDir",true);



Local File System
// param1: baseDire, if specified the files will be found from base dir and below its hierarchy using relative paths, otherwise the full path needs to be passed in

var localSystemProvider= FileSystemProvider( "baseDir");



All operations will work the same for each provider, if you want to switch between them only the constructor takes in different parameters, the rest will look the same




````




