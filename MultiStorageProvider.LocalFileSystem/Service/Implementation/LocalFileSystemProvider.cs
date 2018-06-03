using MultiStorageProvider.Common.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MultiStorageProvider.LocalFileSystem.Service.Implementation
{
    public class FileSystemProvider : IStorageProvider
    {

        private string baseDir;
        public FileSystemProvider(string baseDirectory = null)
        {
            baseDir = baseDirectory;
        }

        private string MapPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            if (string.IsNullOrEmpty(baseDir)) return path;
            return $"{baseDir.TrimEnd(Path.DirectorySeparatorChar)}{Path.DirectorySeparatorChar}{path.TrimStart(Path.DirectorySeparatorChar)}";
        }
        public async Task<bool> FileExists(string fileName)
        {
            return File.Exists(MapPath(fileName));
        }
        public async Task<bool> AddFile(string fileName, string name, bool overrideIfExists = false)
        {
            try
            {
                File.Copy(fileName, MapPath(name), overrideIfExists);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> AddFile(Stream stream, string name, bool overrideIfExists = false)
        {
            try
            {
                if (File.Exists(MapPath(name)) && !overrideIfExists) return true; //if file exist and not set to override return success
                using (var fileStream = File.OpenWrite(MapPath(name)))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }
            }
            catch
            {

                return false;
            }

            return true;

        }
        public async Task<bool> AddFile(byte[] bytes, string name, bool overrideIfExists = false)
        {

            try
            {
                if (File.Exists(MapPath(name)) && !overrideIfExists) return true; //if file exist and not set to override return success
                var path = new FileInfo(MapPath(name));
                path.Directory.Create();
                using (var fileStream = path.Open(FileMode.Create))
                {
                    await fileStream.WriteAsync(bytes, 0, bytes.Length - 1);
                }
            }
            catch
            {
                return false;
            }


            return true;

        }

        public async Task<bool> DeleteFile(string name)
        {
            try
            {
                if (!await FileExists(name)) return false;
                File.Delete(MapPath(name));
                return true;

            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DownloadToFile(string filename, string outputFileName, bool overrideIfExists = false)
        {

            try
            {
                if (!await FileExists(filename)) return false;
                File.Copy(MapPath(filename), outputFileName, overrideIfExists);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task DownloadStream(Stream stream, string name)
        {
            var filestream = File.Open(MapPath(name), FileMode.Open, FileAccess.ReadWrite);
            await filestream.CopyToAsync(stream);
            filestream.Dispose();
        }
        public async Task<byte[]> DownloadBytes(string name)
        {
            if (!await FileExists(name)) return null;
            return File.ReadAllBytes(MapPath(name));

        }
        public async Task<bool> RenameFile(string originalName, string newName, bool overrideIfExists = false)
        {
            if (!await FileExists(originalName)) return false;
            if (!await FileExists(newName) && !overrideIfExists) return false;
            try
            {
                File.Move(MapPath(originalName), MapPath(newName));
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> UpdateFile(Stream stream, string name)
        {
            if (!await FileExists(MapPath(name))) return false;
            return await AddFile(stream, name, true);
        }

        public async Task<bool> UpdateFile(string fileName, string name)
        {
            if (!await FileExists(MapPath(name))) return false;
            return await AddFile(fileName, name, true);

        }

        public async Task<bool> DeleteFolder(string name)
        {
            var dirName = MapPath(name);
            if (!Directory.Exists(dirName)) return false;

            try
            {
                Directory.Delete(dirName, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
