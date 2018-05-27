using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AzureStorageService.Service.Implementation;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using System.IO;

namespace AzureStorageService.Test
{
    [TestClass]
    public class BlobProviderTests
    {
       
        [TestMethod]
        public async Task GetTest()
        {
            BlobShareProvider provider = new BlobShareProvider(CloudStorageAccount.DevelopmentStorageAccount, "test2");
            var filename =Path.GetTempFileName();
            var add =await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename);
            var file =await  provider.DownloadBytes(filename);

            Assert.IsTrue(file.Length == 1000);

        }
    }
}
