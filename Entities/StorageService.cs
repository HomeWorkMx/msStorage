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
        public async Task<string> ImagenGetId(string id)
        {
            try
            {
                string containerName = _configuration.GetValue<string>("AppSettings:ContainerName");
                BlobContainerClient container = new BlobContainerClient(_configuration.GetConnectionString("StorageConnectionString"), containerName);

                BlobClient blob = container.GetBlobClient(containerName);

                if (await blob.ExistsAsync())
                {
                    return  blob.OpenReadAsync().ToString();
                }

                return "NOok";
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
                string blobName = _configuration.GetValue<string>("AppSettings:ContainerName");

                BlobContainerClient container = new BlobContainerClient(_configuration.GetConnectionString("StorageConnectionString"), containerName);
                BlobClient blob = container.GetBlobClient(blobName);

                var salida = await blob.UploadAsync(filePath);

                return salida;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<string> ImagenGetAll()
        {
            try
            {
                string containerName = _configuration.GetValue<string>("AppSettings:ContainerName");
                BlobContainerClient container = new BlobContainerClient(_configuration.GetConnectionString("StorageConnectionString"), containerName);

                await container.CreateIfNotExistsAsync();

                string salida = "";
                foreach (BlobItem blobSt in container.GetBlobs())
                {
                    salida = salida +"--"+ blobSt.Name;
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
