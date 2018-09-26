using MultiStorageProvider.Service.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MultiStorageProvider.LocalFileSystem.Service.Implementation;
using MultiStorageProvider.GoogleDrive.Service.Implementation;

namespace MultiStorageProvider.Test
{

    [TestClass]
    public class GDriveTest
    {

        [TestMethod]
        public async Task Get()
        {
            var files = await new GoogleDriveProvider().AddFile(@"C:\Users\nertil\Source\Repos\AzureStorageService\README.md", "readmefile");

        }

        [TestMethod]
        public async Task List()
        {
        //    var folder = await new GoogleDriveProvider().CreateFolder("testfolder");
            var files = await new GoogleDriveProvider().ListFles();
            var file = await new GoogleDriveProvider().DownloadBytes("");

        }
        
    }
}
