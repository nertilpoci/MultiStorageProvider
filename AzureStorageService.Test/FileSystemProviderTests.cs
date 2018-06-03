using MultiStorageProvider.Service.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiStorageProvider.LocalFileSystem.Service.Implementation;

namespace MultiStorageProvider.Test
{

    [TestClass]
    public class FileSystemProviderTests
    {
        private static string BaseDir = Directory.GetCurrentDirectory() +"\\storagetests";
        [TestCleanup]
        public void CleanUp()
        {
            Directory.Delete(BaseDir,true);
        }

        [TestInitialize]
        public void Init()
        {
            Directory.CreateDirectory(BaseDir);
        }


        [TestMethod]
        public async Task AddBytesTest()
        {
            var provider = new FileSystemProvider(BaseDir);
            var filename = Path.GetRandomFileName();
            var add = await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename);
            Assert.IsTrue(add);
            Assert.IsTrue(await provider.FileExists(filename));

        }

        [TestMethod]
        public async Task AddStreamTest()
        {
            var provider = new FileSystemProvider(BaseDir); var filename = Path.GetRandomFileName();
            using (var stream = new MemoryStream(RandomFileGenerator.RandomFileByteArray(1000)))
            {
                var add = await provider.AddFile(stream, filename);
                Assert.IsTrue(add);
                Assert.IsTrue(await provider.FileExists(filename));
            }


        }

        [TestMethod]
        public async Task AddFileTest()
        {
            var provider = new FileSystemProvider(BaseDir);
            var filename = Path.GetRandomFileName();
            var tempFile = Path.GetTempFileName();
            using (var file = File.OpenWrite(tempFile))
            {
                await file.WriteAsync(RandomFileGenerator.RandomFileByteArray(1000), 0, 1000);

            }

            var add = await provider.AddFile(tempFile, filename);
            Assert.IsTrue(add);
            Assert.IsTrue(await provider.FileExists(filename));
        }
        [TestMethod]
        public async Task GetBytesTest()
        {
            var provider = new FileSystemProvider(BaseDir);
            var filename = Path.GetRandomFileName();
            var add = await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename);
            Assert.IsTrue(await provider.FileExists(filename));

        }
        [TestMethod]
        public async Task GetStreamTest()
        {
            var provider = new FileSystemProvider(BaseDir);
            var filename = Path.GetRandomFileName();
            var add = await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename);

            using (MemoryStream stream = new MemoryStream())
            {
                await provider.DownloadStream(stream, filename);
                Assert.IsTrue(stream.Length == 999);//file storage size is 999 for 1000 bytes
            }

        }
        [TestMethod]
        public async Task GetFileTest()
        {
            var provider = new FileSystemProvider(BaseDir);
            var filename = Path.GetRandomFileName();
            var outputFileName = Path.GetTempFileName();
            var add = await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename);

            await provider.DownloadToFile(filename, outputFileName, true);//overdie if exists since gettempfileme creates the file

            var file = File.ReadAllBytes(outputFileName);
            Assert.IsTrue(await provider.FileExists(filename));
            File.Delete(outputFileName);


        }
        [TestMethod]
        public async Task DeleteTest()
        {
            var provider = new FileSystemProvider(BaseDir);
            var filename = Path.GetRandomFileName();
            var add = await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename);
            Assert.IsTrue(await provider.DeleteFile(filename));
            Assert.IsFalse(await provider.FileExists(filename));

        }

        [TestMethod]
        public async Task DeleteFolderTest()
        {
            var provider = new FileSystemProvider(BaseDir);
            var filename1 = "testfolder\\" + Path.GetRandomFileName();
            var filename2 = "testfolder\\" + Path.GetRandomFileName();
            await provider.AddFile(RandomFileGenerator.RandomFileByteArray(1000), filename1);
            await provider.AddFile(RandomFileGenerator.RandomFileByteArray(2000), filename2);

            var result = await provider.DeleteFolder("testfolder");

            Assert.IsTrue(result);
            Assert.IsFalse(await provider.FileExists(filename1));
            Assert.IsFalse(await provider.FileExists(filename2));
        }

    }
}
