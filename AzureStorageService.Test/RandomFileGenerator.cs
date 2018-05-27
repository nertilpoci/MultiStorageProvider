using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageService.Test
{
    public static class RandomFileGenerator
    {
        public static byte[] RandomFileByteArray(int size)
        {
            byte[] data = new byte[size];
            Random rng = new Random();
            rng.NextBytes(data);
            return data;
        }
        public static Stream RandomFileStream(int size)
        {
            return new MemoryStream(RandomFileByteArray(size));
        }
    }
    
}
