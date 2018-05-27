using System;
using System.Collections.Generic;
using System.Text;

namespace AzureStorageService.Models
{
    public class FileResult
    {
        public bool IsFolder { get; set; }
        public string Name { get; set; }
        
        public string Path { get; set; }
    }
}
