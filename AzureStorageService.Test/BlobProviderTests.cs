using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiStorageProvider.Service.Implementation;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using System.IO;
using MultiStorageProvider.Azure.Service.Implementation;

namespace MultiStorageProvider.Test
{
    [TestClass]
    public class BlobProviderTests
    {
        private static string ContainerName="test";
        [TestCleanup]
        public void CleanUp()
        {
            var bloblClient = CloudStorageAccount.DevelopmentStorageAccount.CreateCloudBlobClient();
            var container = bloblClient.GetContainerReference(ContainerName);
            container.DeleteIfExists();
        }

        [TestInitialize]
        public void Init()
        {
           new BlobShareProvider(CloudStorageAccount.DevelopmentStorageAccount, ContainerName, null,true);

        }


        [TestMethod]
        [DataRow("")]
        [DataRow("basedir")]
        public async Task AddBytesTest(string basedir)
        {
            BlobShareProvider provider = new BlobShareProvider(CloudStorageAccount.DevelopmentStorageAccount, ContainerName, basedir);
            var filename = Path.GetRandomFileName();
            var add = await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename);
            var file = await provider.DownloadBytes(filename);
            Assert.IsTrue(file.Length == 1000);

        }

        [TestMethod]
        [DataRow("")]
        [DataRow("basedir")]
        public async Task AddStreamTest(string baseDir)
        {
            BlobShareProvider provider = new BlobShareProvider(CloudStorageAccount.DevelopmentStorageAccount, ContainerName,baseDir);
            var filename = Path.GetRandomFileName();
            using (var stream=new MemoryStream(RandomFileGenerator.RandomFileByteArray(1000)))
            {
                var add = await provider.AddFile(stream, filename);
                var file = await provider.DownloadBytes(filename);
                Assert.IsTrue(file.Length == 1000);
            }
           

        }

        [TestMethod]
        [DataRow("")]
        [DataRow("basedir")]
        public async Task AddFileTest(string baseDir)
        {
            BlobShareProvider provider = new BlobShareProvider(CloudStorageAccount.DevelopmentStorageAccount, ContainerName,baseDir);
            var filename = Path.GetRandomFileName();
            var tempFile = Path.GetTempFileName();
            using (var file =File.OpenWrite(tempFile))
            {
                await file.WriteAsync(RandomFileGenerator.RandomFileByteArray(1000),0,1000);
               
            }

            var add = await provider.AddFile(tempFile, filename);
            var result = await provider.DownloadBytes(filename);
            Assert.IsTrue(result.Length == 1000);
        }
        [TestMethod]
        [DataRow("")]
        [DataRow("basedir")]
        public async Task GetBytesTest(string baseDir)
        {
            BlobShareProvider provider = new BlobShareProvider(CloudStorageAccount.DevelopmentStorageAccount, ContainerName,baseDir);
            var filename =Path.GetRandomFileName();
            var add =await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename);
            var file =await  provider.DownloadBytes(filename);
            Assert.IsTrue(file.Length == 1000);

        }
        [TestMethod]
        [DataRow("")]
        [DataRow("basedir")]
        public async Task GetStreamTest(string  baseDir)
        {
            BlobShareProvider provider = new BlobShareProvider(CloudStorageAccount.DevelopmentStorageAccount, ContainerName,baseDir);
            var filename = Path.GetRandomFileName();
            var add = await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename);
           
            using( MemoryStream stream = new MemoryStream())
            {
                await provider.DownloadStream(stream, filename);
                Assert.IsTrue(stream.Length == 1000);
            }

        }
        [TestMethod]
        [DataRow("")]
        [DataRow("basedir")]
        public async Task GetFileTest(string baseDir)
        {
            BlobShareProvider provider = new BlobShareProvider(CloudStorageAccount.DevelopmentStorageAccount,ContainerName,baseDir);
            var filename = Path.GetRandomFileName();
            var outputFileName = Path.GetTempFileName();
            var add = await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename);

            await provider.DownloadToFile(filename, outputFileName,true);//overdie if exists since gettempfileme creates the file

            var file = File.ReadAllBytes(outputFileName);
            Assert.IsTrue(file.Length == 1000);
            File.Delete(outputFileName);


        }
        [TestMethod]
        [DataRow("")]
        [DataRow("basedir")]
        public async Task DeleteTest(string baseDir)
        {
            BlobShareProvider provider = new BlobShareProvider(CloudStorageAccount.DevelopmentStorageAccount,ContainerName,baseDir);
            var filename = Path.GetRandomFileName();
            var add = await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename);
            var file = await provider.DownloadBytes(filename);
            Assert.IsTrue(file.Length == 1000);
            Assert.IsTrue(await provider.DeleteFile(filename));
            Assert.IsFalse(await provider.FileExists(filename));

        }

        [TestMethod]
        [DataRow("")]
        [DataRow("basedir")]
        public async Task DeleteFolderTest(string baseDir)
        {
            BlobShareProvider provider = new BlobShareProvider(CloudStorageAccount.DevelopmentStorageAccount, ContainerName,baseDir);
            var filename1 ="testfolder\\" + Path.GetRandomFileName();
            var filename2 ="testfolder\\" + Path.GetRandomFileName();
            await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename1);
            await provider.AddFile(RandomFileGenerator.RandomFileByteArray(2000), filename2);

            var result =await provider.DeleteFolder("testfolder");

            Assert.IsTrue(result);
            Assert.IsFalse(await provider.FileExists(filename1));
            Assert.IsFalse(await provider.FileExists(filename2));
        }

    }
}
