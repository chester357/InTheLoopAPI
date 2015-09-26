using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;

namespace InTheLoopAPI.DAL
{
    public class BlobContext
    {
        public CloudBlobContainer Container;

        public BlobContext()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

             Container = blobClient.GetContainerReference("images-dev");
        }

        public String UploadImage(String imageName, String uploadPath)
        {
            CloudBlockBlob blockBlob = Container.GetBlockBlobReference(imageName);

            using (var fileStream = System.IO.File.OpenRead(uploadPath))
            {
                blockBlob.UploadFromStream(fileStream);
            }

            return "https://intheloop.blob.core.windows.net/images-dev/" + imageName;
        }

        public String UploadImage(HttpPostedFile file, String imageName)
        {
            CloudBlockBlob blockBlob = Container.GetBlockBlobReference(imageName);

            using (var fileStream = file.InputStream)
            {
                blockBlob.UploadFromStream(fileStream);
            }

            return "https://intheloop.blob.core.windows.net/images-dev/" + imageName;
        }
     
    }
}