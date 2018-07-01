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
        public async Task AddBytesTest()
        {
            var files = await new GoogleDriveProvider().ListFles();

        }
        
    }
}
