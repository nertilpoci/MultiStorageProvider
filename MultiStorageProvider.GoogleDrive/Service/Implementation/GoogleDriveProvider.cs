using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using MultiStorageProvider.Common.Service.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MultiStorageProvider.GoogleDrive.Service.Implementation
{
    public class GoogleDriveProvider : IStorageProvider
    {
        DriveService service;
        private string _parentId;
        public GoogleDriveProvider()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive };

            var keyFilePath = @"C:\Users\nertil\Documents\service.json";   
            GoogleCredential credential;
            using (var stream = new FileStream(keyFilePath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                     .CreateScoped(scopes);
            }
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Storage",
            });
        }

        public async Task<bool> AddFile(byte[] bytes, string name, bool overrideIfExists = false)
        {
            return await AddFile(new MemoryStream(bytes), name, overrideIfExists);
        }

        public async Task<bool> AddFile(Stream stream, string name, bool overrideIfExists = false)
        {
            var body = CreateBody(name);
            try
            {
                var id = service.Files.GenerateIds().Execute();
                var request = service.Files.Create(body, stream, GetMimeType(name));
                var result=await request.UploadAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
       
        public async Task<bool> AddFile(string fileName, string name, bool overrideIfExists = false)
        {
            if (System.IO.File.Exists(fileName))
            {
                byte[] byteArray = System.IO.File.ReadAllBytes(fileName);
                return await AddFile(byteArray, name, overrideIfExists);         
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteFile(string name)
        {
            try
            {
                FilesResource.DeleteRequest DeleteRequest = service.Files.Delete(name);
                await DeleteRequest.ExecuteAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Task<bool> DeleteFolder(string name)
        {
            throw new NotImplementedException();
        }
       

        public async Task<byte[]> DownloadBytes(string name)
        {

            return await service.HttpClient.GetByteArrayAsync(name);
        }

        public async Task DownloadStream(Stream stream, string name)
        {
            stream = await service.HttpClient.GetStreamAsync(name);
        }

        public async Task<bool> DownloadToFile(string blobName, string outputFileName, bool overrideIfExists = false)
        {
            if (!overrideIfExists && System.IO.File.Exists(outputFileName)) return false;

            try
            {
                System.IO.File.WriteAllBytes(outputFileName, await DownloadBytes(blobName));
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> FileExists(string fileName)
        {
            try
            {
                var file = await service.Files.Get(fileName).ExecuteAsync();
                return file != null;
            }
            catch (Exception ex)
            {

                return false;
            }
           
        }

        public Task<bool> RenameFile(string originalName, string newName, bool overrideIfExists = false)
        {
            throw new NotImplementedException();
        }

    
        public async Task<bool> UpdateFile(byte[] bytes, string name)
        {
            return await UpdateFile(new MemoryStream(bytes), name);
        }

        public async Task<bool> UpdateFile(Stream stream, string name)
        {
            var body = CreateBody(name);
            try
            {
                var request = service.Files.Update(body,name, stream, GetMimeType(name));
                await request.UploadAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateFile(string fileName, string name)
        {
            if (System.IO.File.Exists(fileName))
            {
                byte[] byteArray = System.IO.File.ReadAllBytes(fileName);
                return await UpdateFile(byteArray, name);
            }
            else
            {
                return false;
            }
        }
        public async Task<IEnumerable<Google.Apis.Drive.v3.Data.File>> ListFles()
        {
            var Files = new List<Google.Apis.Drive.v3.Data.File>();

            try
            {
                
                FilesResource.ListRequest list = service.Files.List();
                FileList filesFeed = list.Execute();

                //// Loop through until we arrive at an empty page
                while (filesFeed.Files != null)
                {
                    // Adding each item  to the list.
                    foreach (Google.Apis.Drive.v3.Data.File item in filesFeed.Files)
                    {
                        Files.Add(item);
                    }

                    // We will know we are on the last page when the next page token is
                    // null.
                    // If this is the case, break.
                    if (filesFeed.NextPageToken == null)
                    {
                        break;
                    }

                    // Prepare the next page of results
                    list.PageToken = filesFeed.NextPageToken;

                    // Execute and process the next page request
                    filesFeed = list.Execute();
                }
            }
            catch (Exception ex)
            {
                // In the event there is an error with the request.
                Console.WriteLine(ex.Message);
            }
            return Files;
        }
        private string GetMimeType(string fileName = "")
        {
            string mimeType = "application/unknown";

            return mimeType;
        }
        private Google.Apis.Drive.v3.Data.File CreateBody(string name)
        {

            Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
            body.Name = Path.GetFileName(name);
            body.Id = "1HHixJuQHl0dAWt0TKxhLz1_hhJ5usO-c";
            body.Description = "File uploaded by MultiStoragerovider";
            return body;
        }

         public async Task<Google.Apis.Drive.v3.Data.File> CreateFolder(string name)
        {

            Google.Apis.Drive.v3.Data.File body = new Google.Apis.Drive.v3.Data.File();
            body.Name=  "testfolder";
            body.Description = "document description";
            body.MimeType = "application/vnd.google-apps.folder";

            // service is an authorized Drive API service instance
           var result=await service.Files.Create(body).ExecuteAsync();
            return result;
        }
    }
}
