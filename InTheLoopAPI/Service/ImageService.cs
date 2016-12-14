using InTheLoopAPI.DAL;
using InTheLoopAPI.Models;
using InTheLoopAPI.Models.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace InTheLoopAPI.Service
{
    public class ImageService
    {
        BlobContext BlobContext = new BlobContext();
        DatabaseContext DataContext = new DatabaseContext();

        public String UploadImage(HttpFileCollection files, String userId)
        {
            if (files.Count != 1) throw new Exception("Invalid File Count");

            HttpPostedFile file = files[0];

            if (file.ContentType.Substring(0, 5) != "image") throw new Exception("Invalid content type");

            var fileExtension = file.FileName.Substring(file.FileName.Length - 4, 4);

            var imageName = userId + DateTime.Now.ToString().Replace(" ", "") + fileExtension;

            var path = BlobContext.UploadImage(file, imageName);

            return path;
        }

        public void UpdateProfileImage(String userId, String imageURL)
        {
            var user = DataContext.MyUsers.Single(x => x.Id == userId);

            user.ImageURL = imageURL;

            DataContext.SaveChangesAsync();
        }

        public String UploadImage(HttpFileCollection files)
        {
            if (files.Count != 1) throw new Exception("Invalid File Count");

            HttpPostedFile file = files[0];

            if (file.ContentType.Substring(0, 5) != "image") throw new Exception("Invalid content type");

            var path = BlobContext.UploadImage(file, file.FileName);

            return path;
        }

        public List<StockPhoto> GetStockPhotos(string Category)
        {
            return DataContext.StockPhotos
                .Where(x => x.Category == Category)
                .ToList();
        }
     }
}