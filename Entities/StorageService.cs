using System;
using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
//using System.Configuration;
using Azure.Storage.Blobs;

using Azure.Storage.Blobs.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace msStorage
{
    public class StorageService
    {
       
        public readonly IConfiguration _configuration;
        public StorageService(IConfiguration _Configuration)
        {
            _configuration = _Configuration;
        }
        public async Task<Azure.Response> PdfGetNombre(string nombreArchivo, string downloadPath)
        {
            try
            {
                string containerName = _configuration.GetValue<string>("AppSettings:ContainerName");
                string blobName = _configuration.GetValue<string>("AppSettings:PdfBlob");
                BlobContainerClient container = new BlobContainerClient(_configuration.GetConnectionString("StorageConnectionString"), containerName);
                BlobClient blob = container.GetBlobClient(nombreArchivo);
                if (await blob.ExistsAsync())
                {

                    using (var fileStream = System.IO.File.OpenWrite(downloadPath + "\\"+ nombreArchivo))
                    {
                        var salida = await blob.DownloadToAsync(fileStream);
                        return salida;
                    }

                   
                }
                else {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<Azure.Response<Azure.Storage.Blobs.Models.BlobContentInfo>> ImagenSave(string filePath)
        {
            try
            {
                string containerName = _configuration.GetValue<string>("AppSettings:ContainerName");
                string blobName = _configuration.GetValue<string>("AppSettings:PdfBlob");
                string nomArchivo = filePath.Split("\\")[filePath.Split("\\").Length-1];
                BlobContainerClient container = new BlobContainerClient(_configuration.GetConnectionString("StorageConnectionString"), containerName);
                BlobClient blob = container.GetBlobClient(blobName+ nomArchivo);
                var salida = await blob.UploadAsync(filePath);
                return salida;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable< Azure.Page<BlobItem>>> ImagenGetAll()
        {
            try
            {
                string containerName = _configuration.GetValue<string>("AppSettings:ContainerName");
                BlobContainerClient container = new BlobContainerClient(_configuration.GetConnectionString("StorageConnectionString"), containerName);
                await container.CreateIfNotExistsAsync();
                var salida = container.GetBlobs().AsPages(default, null);
                string ver = "";
                foreach (Azure.Page< BlobItem> blobPage in salida)
                {
                    foreach (BlobItem blobItem in blobPage.Values)
                    {
                        ver = ver + "--" + blobItem.Name;
                    }
                }

                return salida;
            }
            catch (Exception ex)
            {

                throw;
            }
        }





    }
}
