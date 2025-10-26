using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BusinessLogic.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        List<string> allowedExtension = [".png", ".jpg", ".jpeg"];
        const int maxSize = 2_097_152;
        public string? Upload(IFormFile file, string folderName)
        {
            var extension = Path.GetExtension(file.FileName);
            if(!allowedExtension.Contains(extension)) return null;
            if (file.Length > maxSize || file.Length == 0) return null;
            //F:\HEMA\VProjects\DemoSolution\Demo.Presentation\wwwroot\files\images
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(folderPath, fileName);
            using FileStream fs = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fs);
            return fileName;
        }
        public bool Delete(string filePath)
        {
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

    }
}
